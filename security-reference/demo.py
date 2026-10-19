"""Local cryptography demonstration only. NO real purchase is validated."""
from cryptography.hazmat.primitives.asymmetric import rsa
from licensing import *
def main():
    print('DEMO LOCAL — resultado de loja simulado, proibido em produção.')
    key=rsa.generate_private_key(public_exponent=65537,key_size=3072)
    clock=lambda: 1802865600
    challenges=ChallengeStore(clock); installation='demo-install-01'
    challenge=challenges.issue(installation)
    digest=request_hash(challenge,installation,'android','com.soldecobre.game','DEMO','demo-key-thumbprint','renew')
    fake=StoreResult(True,'com.soldecobre.game','android','demo-subject',installation,digest,int(clock()),True)
    issuer=Issuer(key,'demo-key','com.soldecobre.game','android',challenges,clock)
    token=issuer.issue(fake,challenge,installation,digest)
    result=Verifier({'demo-key':key.public_key()},'com.soldecobre.game','android',clock).verify(token,installation)
    print('Assinatura, vínculo e prazo verificados:',result.mode)
    print('Chave privada efêmera não foi salva. Adaptadores de loja não incluídos.')
if __name__=='__main__': main()
