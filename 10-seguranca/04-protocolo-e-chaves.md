# Envelope de licença e ciclo de chaves

## Formato implementado

`sc1.<kid>.<payload-base64url>.<signature-base64url>`

Assinatura RSA PKCS#1 v1.5 com SHA-256 e módulo de pelo menos 3072 bits cobre os bytes ASCII exatos dos três primeiros segmentos. Base64url sem padding e canônico. Algoritmo fixo por versão `sc1`; não há cabeçalho controlado pelo cliente que selecione `none`, HMAC ou outra chave. Limite de token: 8192 caracteres. Payload JSON ASCII canônico: chaves ordenadas, sem espaços, apenas campos permitidos, sem campos duplicados ou extras.

| Campo | Regra |
|---|---|
| `v` | Inteiro 1, booleano não é aceito |
| `app` | ID configurado; igualdade exata |
| `platform` | `android` ou `ios`; igualdade exata |
| `subject` | Identificador pseudônimo interno desta concessão; não é e-mail da loja |
| `installation` | Instalação esperada autenticada pelo adaptador |
| `jti` | Aleatório 128 bits; índice de auditoria e revogação |
| `iat` | Unix UTC inteiro; emissão não no futuro além de 300 s |
| `exp` | Prazo normal, 30 dias no emissor |
| `offline_until` | Limite absoluto; no máximo 37 dias desde `iat` |

Strings têm 1–128 caracteres no alfabeto ASCII `[A-Za-z0-9_.:-]`. Verificação de assinatura vem antes de uso de claims. Payload deve reserializar identicamente; isso elimina interpretações de duplicatas e formas não canônicas. Cliente e servidor compartilham vetores positivos/negativos de interoperabilidade antes do release. O verificador C# completo ainda deve ser integrado e compilado; as interfaces incluídas não declaram um token válido sozinhas.

## Chaves e rotação

KMS gerencia chave privada de licença, separada da assinatura das lojas, do CDN e de TLS. Aplicativo leva chaves públicas allowlisted. `kid` é ID opaco, nunca caminho ou URL buscável informado pelo cliente. Pré-distribuir chave pública nova, aguardar adoção de versão mínima, emitir pela nova chave e manter a antiga durante a maior validade emitida mais janela de rollout. Nunca remover antiga em um release que ainda precisa aceitar licenças legítimas existentes.

Em comprometimento: interromper emissor, trocar credenciais, revogar acesso ao KMS, distribuir política/chaves por atualização assinada e atendimento. Uma cópia offline não consulta a lista nova até reconectar: essa é uma exposição deliberada de até 37 dias. Não prometer revogação instantânea offline.

## API e armazenamento

Licença não é senha universal ou bearer administrativo. API exige principal de instalação autenticado e assinatura da operação. Armazenar cache no Keystore/Keychain quando viável; chave privada do emissor jamais sai do backend. Relógio protegido reduz fraude casual, mas não é raiz de confiança absoluta em aparelho comprometido.

## Idempotência

`Idempotency-Key` autentica-se com o principal e digest do corpo. Uma mesma chave com o mesmo corpo retorna resposta persistida por 24 h; mesma chave com corpo diferente retorna 409. Desafio só pode emitir uma concessão. Atualização da licença e consumo devem ser transação ou outbox que permita recuperar o resultado assinado em crash. A referência em memória demonstra atomicidade de consumo, não durabilidade distribuída.
