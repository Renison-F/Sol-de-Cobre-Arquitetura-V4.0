# Contratos e fonte de dados V3

`campanha.json` contém as dez fases, seis seções por fase, falas, flags e ataques. `elenco.json` e `bestiario.json` são inventários de identidade e arte. `save.schema.json` define formato alvo; os scripts C# da V2 continuam um exemplo menor e exigem migração antes do uso final. `license.schema.json` ajuda validar estrutura, mas não valida assinatura ou prazo relativo.

OpenAPI descreve endpoints a implementar, usando domínio `.invalid` reservado. Não é endereço funcional e não existe cliente apontado a ele. SQL é migração inicial de referência, não declaração de banco provisionado. Compor inscrição autenticada, Redis e transação/outbox conforme segurança antes de servir solicitações reais.

Hash de operação: JSON ASCII canônico com chaves `app`, `challenge`, `installation`, `key_thumbprint`, `operation`, `platform`, `proof_sha256`; `proof_sha256` é SHA-256 hex da prova de compra relevante. Calcular SHA-256 desses bytes e codificar base64url sem padding. `platform_proof` é produzido sobre esse digest, logo não entra no próprio digest (evita dependência circular). Android premium usa prova de compra e integridade combinada no veredito oficial: nesse caso `store_proof` é vazio e seu hash é o hash da string vazia. No iOS o campo recebe o dado assinado de AppTransaction. O bridge e servidor precisam compartilhar vetores antes de produção.

Matrícula usa `enrollment_public_key`, e o servidor verifica que seu thumbprint coincide com o campo vinculado à prova e que há posse da chave. Renovação/restauração autenticadas não aceitam redefinir a chave. Propriedades condicionais por operação são verificadas no serviço além do schema. `possession_signature` assina os mesmos bytes canônicos com algoritmo definido pela chave da instalação, validado por adapter; não pelo algoritmo da licença. Credencial de API expira em 900 s e se renova por prova de posse + desafio; licença offline não funciona como credencial administrativa.

O gateway tem teto total de 64 KiB mesmo quando limites de campos individuais permitiriam soma maior. Erros não devem revelar prova ou segredos. Rejeitar propriedades desconhecidas e codificação inválida antes de acessar fornecedor externo.

Renovação da credencial curta: usar matrícula verificada novamente com a mesma chave/instalação (`enroll`), sem substituir vínculo. Para ID já existente, a mesma chave autenticada recebe nova sessão de 900 s; outra chave recebe 409 e segue reativação em nova instalação. O endpoint de renovar licença continua exigindo sessão atual. Não existe refresh token indefinido no cliente.


## Produção 3D V4

`11-contratos/modelagem.json` é a fonte numérica de 17 pacotes de produção. `11-contratos/modelagem-resumo.csv` permite consulta tabular; `11-contratos/handoff.template.json` é um modelo não preenchido, que deve falhar no validador até receber medições/arquivos reais. Nenhum status de malha foi marcado como pronto.

`11-contratos/modelagem-secundarios.json` detalha metas dos 19 inimigos e das três famílias de figurantes, sem declarar modelos produzidos.
