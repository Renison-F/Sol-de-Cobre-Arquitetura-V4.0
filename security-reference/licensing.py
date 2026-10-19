"""Reference core: NOT an Internet-facing service or store-verification SDK.
Install requirements, run python -m unittest discover -s tests -v.
Only production adapters may create a verified store result. No HTTP adapter is supplied.
"""
import base64, hashlib, json, re, secrets, threading, time
from dataclasses import dataclass
from cryptography.hazmat.primitives import hashes
from cryptography.hazmat.primitives.asymmetric import padding, rsa
from cryptography.exceptions import InvalidSignature

DAY=86400
MAX_LEASE=37*DAY
SAFE=re.compile(r'^[A-Za-z0-9_.:-]{1,128}$')
class LicenseError(ValueError): pass

def b64(data): return base64.urlsafe_b64encode(data).rstrip(b'=').decode('ascii')
def unb64(value):
    if not isinstance(value,str) or not re.fullmatch(r'[A-Za-z0-9_-]+',value):
        raise LicenseError('encoding')
    try: data=base64.urlsafe_b64decode(value+'='*((-len(value))%4))
    except Exception as e: raise LicenseError('encoding') from e
    if b64(data)!=value: raise LicenseError('encoding')
    return data

def canonical(data):
    return json.dumps(data,sort_keys=True,separators=(',',':'),ensure_ascii=True).encode('ascii')

def request_hash(challenge,installation,platform,app,proof,key_thumbprint,operation):
    # Bind the complete store proof, never log it. Same JSON bytes in native adapters.
    return b64(hashlib.sha256(canonical({'app':app,'challenge':challenge,
        'installation':installation,'key_thumbprint':key_thumbprint,
        'operation':operation,'platform':platform,'proof_sha256':hashlib.sha256(proof.encode('utf-8')).hexdigest()})).digest())

@dataclass(frozen=True)
class StoreResult:
    # Internal boundary: construct ONLY after authentic store verification.
    licensed: bool
    app: str
    platform: str
    subject: str
    installation: str
    request_hash: str
    verified_at: int
    production: bool
    revoked: bool=False

class ChallengeStore:
    """Single-process reference. Production: Redis GETDEL/Lua, TTL + authenticated principal."""
    def __init__(self,clock=time.time): self.clock=clock; self.items={}; self.lock=threading.Lock()
    def issue(self,installation):
        if not SAFE.fullmatch(installation): raise LicenseError('installation')
        now=int(self.clock())
        with self.lock:
            self.items={k:v for k,v in self.items.items() if v[1]>now}
            if len(self.items)>=10000: raise LicenseError('capacity')
            nonce=secrets.token_urlsafe(32); self.items[nonce]=(installation,now+120)
        return nonce
    def consume(self,nonce,installation):
        # Wrong installation cannot consume another installation's challenge.
        with self.lock:
            entry=self.items.get(nonce)
            if not entry or entry[0]!=installation or entry[1]<=int(self.clock()):
                raise LicenseError('challenge')
            del self.items[nonce]

class Issuer:
    def __init__(self,key,kid,app,platform,challenges,clock=time.time):
        if not isinstance(key,rsa.RSAPrivateKey) or key.key_size<3072: raise LicenseError('key')
        for v in (kid,app,platform):
            if not SAFE.fullmatch(v): raise LicenseError('configuration')
        if platform not in ('android','ios'): raise LicenseError('platform')
        self.key,self.kid,self.app,self.platform,self.challenges,self.clock=key,kid,app,platform,challenges,clock
    def issue(self,result,challenge,installation,expected_hash):
        now=int(self.clock())
        if (not result.licensed or result.revoked or not result.production or
            result.app!=self.app or result.platform!=self.platform or
            result.installation!=installation or result.request_hash!=expected_hash or
            not SAFE.fullmatch(result.subject) or
            abs(now-result.verified_at)>120): raise LicenseError('store_verdict')
        self.challenges.consume(challenge,installation)
        claims={'app':self.app,'exp':now+30*DAY,'iat':now,'installation':installation,
            'jti':secrets.token_hex(16),'offline_until':now+37*DAY,'platform':self.platform,
            'subject':result.subject,'v':1}
        signing=('sc1.'+self.kid+'.'+b64(canonical(claims))).encode('ascii')
        signature=self.key.sign(signing,padding.PKCS1v15(),hashes.SHA256())
        return signing.decode('ascii')+'.'+b64(signature)

@dataclass(frozen=True)
class VerifiedLicense:
    claims: dict
    mode: str

class Verifier:
    def __init__(self,public_keys,app,platform,clock=time.time):
        self.keys,self.app,self.platform,self.clock=public_keys,app,platform,clock
    def verify(self,token,installation,last_trusted_time=0,revoked_ids=frozenset()):
        if not isinstance(token,str) or len(token)>8192: raise LicenseError('token')
        parts=token.split('.')
        if len(parts)!=4: raise LicenseError('token')
        ver,kid,payload,sig=parts
        if ver!='sc1' or kid not in self.keys: raise LicenseError('key')
        key=self.keys[kid]
        if not isinstance(key,rsa.RSAPublicKey) or key.key_size<3072: raise LicenseError('key')
        try:key.verify(unb64(sig),('.'.join(parts[:3])).encode('ascii'),padding.PKCS1v15(),hashes.SHA256())
        except InvalidSignature as e: raise LicenseError('signature') from e
        raw=unb64(payload)
        try: c=json.loads(raw)
        except Exception as e: raise LicenseError('claims') from e
        fields={'app','exp','iat','installation','jti','offline_until','platform','subject','v'}
        if not isinstance(c,dict) or set(c)!=fields or canonical(c)!=raw: raise LicenseError('claims')
        if any(type(c[k]) is not int for k in ('v','iat','exp','offline_until')): raise LicenseError('claims')
        if any(not isinstance(c[k],str) or not SAFE.fullmatch(c[k]) for k in ('app','installation','jti','platform','subject')): raise LicenseError('claims')
        if c['v']!=1 or c['app']!=self.app or c['platform']!=self.platform or c['installation']!=installation:
            raise LicenseError('binding')
        now=int(self.clock())
        # Trusted time anchor must itself be protected; local rollback is ultimately bypassable.
        if now+300<last_trusted_time: raise LicenseError('clock_rollback')
        if not 0<=c['iat']<=now+300 or not c['iat']<c['exp']<=c['offline_until']<=c['iat']+MAX_LEASE:
            raise LicenseError('time')
        if c['jti'] in revoked_ids: raise LicenseError('revoked')
        if now>=c['offline_until']: raise LicenseError('expired')
        return VerifiedLicense(c,'grace' if now>=c['exp'] else 'valid')
