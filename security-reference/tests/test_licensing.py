import concurrent.futures, json, unittest
from dataclasses import replace
from cryptography.hazmat.primitives.asymmetric import rsa,padding
from cryptography.hazmat.primitives import hashes
from licensing import *
class LicensingTests(unittest.TestCase):
    @classmethod
    def setUpClass(cls): cls.key=rsa.generate_private_key(public_exponent=65537,key_size=3072)
    def setUp(self):
        self.now=1802865600; self.clock=lambda:self.now; self.install='installation-a'
        self.cs=ChallengeStore(self.clock); self.issuer=Issuer(self.key,'key-a','com.soldecobre.game','android',self.cs,self.clock)
        self.verifier=Verifier({'key-a':self.key.public_key()},'com.soldecobre.game','android',self.clock)
    def fixture(self,**changes):
        nonce=self.cs.issue(self.install); digest=request_hash(nonce,self.install,'android','com.soldecobre.game','opaque-proof','key-a-thumbprint','renew')
        verdict=replace(StoreResult(True,'com.soldecobre.game','android','subject-a',self.install,digest,self.now,True),**changes)
        return nonce,digest,verdict
    def issue(self):
        n,h,v=self.fixture();return self.issuer.issue(v,n,self.install,h)
    def signed(self,claims):
        raw='sc1.key-a.'+b64(canonical(claims));return raw+'.'+b64(self.key.sign(raw.encode(),padding.PKCS1v15(),hashes.SHA256()))
    def test_valid(self):self.assertEqual(self.verifier.verify(self.issue(),self.install).mode,'valid')
    def test_tamper(self):
        t=self.issue().split('.');c=json.loads(unb64(t[2]));c['installation']='installation-b';t[2]=b64(canonical(c))
        with self.assertRaises(LicenseError):self.verifier.verify('.'.join(t),'installation-b')
    def test_wrong_installation(self):
        with self.assertRaises(LicenseError):self.verifier.verify(self.issue(),'installation-b')
    def test_unknown_key(self):
        with self.assertRaises(LicenseError):self.verifier.verify(self.issue().replace('.key-a.','.other.'),self.install)
    def test_wrong_app(self):
        with self.assertRaises(LicenseError):Verifier({'key-a':self.key.public_key()},'wrong.app','android',self.clock).verify(self.issue(),self.install)
    def test_wrong_platform(self):
        with self.assertRaises(LicenseError):Verifier({'key-a':self.key.public_key()},'com.soldecobre.game','ios',self.clock).verify(self.issue(),self.install)
    def test_grace_and_expiry(self):
        t=self.issue();self.now+=30*DAY
        self.assertEqual(self.verifier.verify(t,self.install).mode,'grace');self.now+=7*DAY
        with self.assertRaises(LicenseError):self.verifier.verify(t,self.install)
    def test_rollback(self):
        t=self.issue()
        with self.assertRaises(LicenseError):self.verifier.verify(t,self.install,last_trusted_time=self.now+301)
    def test_revocation(self):
        t=self.issue();j=json.loads(unb64(t.split('.')[2]))['jti']
        with self.assertRaises(LicenseError):self.verifier.verify(t,self.install,revoked_ids={j})
    def test_store_denials(self):
        for change in [dict(licensed=False),dict(revoked=True),dict(production=False),dict(app='wrong.app'),dict(platform='ios'),dict(installation='other'),dict(request_hash='wrong'),dict(verified_at=self.now-121)]:
            n,h,v=self.fixture(**change)
            with self.subTest(change=change),self.assertRaises(LicenseError):self.issuer.issue(v,n,self.install,h)
    def test_replay(self):
        n,h,v=self.fixture();self.issuer.issue(v,n,self.install,h)
        with self.assertRaises(LicenseError):self.issuer.issue(v,n,self.install,h)
    def test_challenge_expiry(self):
        n,h,v=self.fixture();self.now+=120
        with self.assertRaises(LicenseError):self.issuer.issue(replace(v,verified_at=self.now),n,self.install,h)
    def test_concurrent_one_winner(self):
        n,h,v=self.fixture()
        def run(_):
            try:self.issuer.issue(v,n,self.install,h);return 1
            except LicenseError:return 0
        with concurrent.futures.ThreadPoolExecutor(max_workers=8) as ex:self.assertEqual(sum(ex.map(run,range(8))),1)
    def test_invalid_claim_types(self):
        t=self.issue();c=json.loads(unb64(t.split('.')[2]));c['v']=True
        with self.assertRaises(LicenseError):self.verifier.verify(self.signed(c),self.install)
    def test_signed_overlong_lease(self):
        c=json.loads(unb64(self.issue().split('.')[2]));c['offline_until']=self.now+MAX_LEASE+1
        with self.assertRaises(LicenseError):self.verifier.verify(self.signed(c),self.install)
    def test_signed_extra_field(self):
        c=json.loads(unb64(self.issue().split('.')[2]));c['admin']=True
        with self.assertRaises(LicenseError):self.verifier.verify(self.signed(c),self.install)
    def test_malformed(self):
        for t in ['', 'x'*8193,'sc1..a.b','sc1.key-a.@@.??','a.b.c']:
            with self.subTest(token=t[:30]),self.assertRaises(LicenseError):self.verifier.verify(t,self.install)
    def test_binding_hash(self):
        self.assertNotEqual(request_hash('a','i','android','app','p','thumb','renew'),request_hash('b','i','android','app','p','thumb','renew'))
        self.assertNotEqual(request_hash('a','i','android','app','p','thumb','renew'),request_hash('a','i','android','app','q','thumb','renew'))
    def test_wrong_install_cannot_burn_nonce(self):
        n=self.cs.issue(self.install)
        with self.assertRaises(LicenseError):self.cs.consume(n,'other')
        self.cs.consume(n,self.install)

    def test_signed_noncanonical_payload(self):
        c=json.loads(unb64(self.issue().split('.')[2]));raw='sc1.key-a.'+b64(json.dumps(c,indent=2).encode())
        token=raw+'.'+b64(self.key.sign(raw.encode(),padding.PKCS1v15(),hashes.SHA256()))
        with self.assertRaises(LicenseError):self.verifier.verify(token,self.install)
    def test_signed_duplicate_payload(self):
        t=self.issue().split('.');raw_claims=unb64(t[2]);raw_claims=raw_claims[:-1]+b',"v":1}'
        raw='sc1.key-a.'+b64(raw_claims);token=raw+'.'+b64(self.key.sign(raw.encode(),padding.PKCS1v15(),hashes.SHA256()))
        with self.assertRaises(LicenseError):self.verifier.verify(token,self.install)
    def test_future_issuance(self):
        c=json.loads(unb64(self.issue().split('.')[2]));c['iat']=self.now+301
        with self.assertRaises(LicenseError):self.verifier.verify(self.signed(c),self.install)
    def test_hash_key_and_operation_binding(self):
        a=request_hash('n','i','android','app','p','key-a','renew')
        self.assertNotEqual(a,request_hash('n','i','android','app','p','key-b','renew'))
        self.assertNotEqual(a,request_hash('n','i','android','app','p','key-a','restore'))
    def test_altered_signature(self):
        t=self.issue().split('.');sig=bytearray(unb64(t[3]));sig[0]^=1;t[3]=b64(bytes(sig))
        with self.assertRaises(LicenseError):self.verifier.verify('.'.join(t),self.install)
    def test_weak_public_key(self):
        key=rsa.generate_private_key(public_exponent=65537,key_size=2048)
        v=Verifier({'key-a':key.public_key()},'com.soldecobre.game','android',self.clock)
        with self.assertRaises(LicenseError):v.verify(self.issue(),self.install)

if __name__=='__main__':unittest.main()
