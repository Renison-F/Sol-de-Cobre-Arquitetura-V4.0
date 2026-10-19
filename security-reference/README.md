# Núcleo de licenciamento executável

Implementação de referência em Python: assinatura RSA-3072/SHA-256, licença com vínculo de instalação, desafio de uso único, validade, tolerância offline limitada e rejeição de adulteração. Não é servidor público nem substitui Play Integrity ou App Attest.

## Rodar

```bash
python -m venv .venv
# Windows: .venv\Scripts\activate
# Linux/macOS: source .venv/bin/activate
python -m pip install -r requirements.txt
python demo.py
python -m unittest discover -s tests -v
```

O demo usa resultado de loja simulado explicitamente e chave privada efêmera na memória. Não exporta chave ou credencial. Produção não deve expor `StoreResult` em entrada HTTP. Adaptadores internos precisam verificar as respostas assinadas reais antes de criar esse objeto. O emissor assume essa fronteira autenticada, como um repositório assume seu banco confiável.

## Limites concretos

Ainda não há autenticação de instalação, Redis/PostgreSQL, KMS, gateway, integrações de loja, iOS nativo ou endpoints de nuvem neste diretório. Os contratos completos para implementar esses adaptadores estão em `10-seguranca/` e `11-contratos/`. Não implantar o demo na internet. Executar os testes prova as invariantes do núcleo, não resistência de um binário Unity.

RSA PKCS#1 v1.5 é usado exclusivamente em assinaturas SHA-256 com chave de pelo menos 3072 bits, não para criptografar dados. O envelope versionado `sc1` tem algoritmo fixo; migração criptográfica exige nova versão. Chaves privadas pertencem ao KMS do backend. O cliente recebe somente chaves públicas homologadas. TLS continua obrigatório.
