# Testes de segurança e referências

## Núcleo executado neste pacote

A suíte em `security-reference/tests/` verifica assinatura e alteração de payload, app/plataforma/instalação divergentes, chave desconhecida, expiração e tolerância, rollback de relógio, revogação conhecida, tipos/campos inválidos, prazo excessivo, desafio expirado, replay e concorrência. O relatório em `08-qualidade/RELATORIO-V3.md` registra a contagem final real. Nenhum teste simulado de compra vale como teste de loja.

## Matriz ainda a executar durante implementação

| ID | Ensaio | Aceite |
|---|---|---|
| SEC-QA-01 | APK reempacotado acessa API real | Backend rejeita prova sem alterar save |
| SEC-QA-02 | JWS Apple modificado / ambiente errado | Nenhuma concessão emitida |
| SEC-QA-03 | Dois requests disputam desafio entre instâncias | Uma concessão durável, resposta recuperável |
| SEC-QA-04 | Assertion iOS repetida e fora de ordem | Replay rejeitado, retry legítimo possível |
| SEC-QA-05 | IDOR em instalação/licença | Principal A não lê ou renova B |
| SEC-QA-06 | Falha KMS após consumo reservado | Outbox recupera mesma concessão, sem duplicação |
| SEC-QA-07 | Falha de loja por 48 h | Cache válido joga e save persiste |
| SEC-QA-08 | Renovação durante pausa ou morte | Sem travar física; retomada coerente |
| SEC-QA-09 | Chave pública nova e antiga | Rollout aceita clientes previstos e rejeita kid desconhecido |
| SEC-QA-10 | Suporte e administração | RBAC, MFA, auditoria e isolamento |
| SEC-QA-11 | Reembolso/compra familiar/reinstalação | Resultado conforme prova oficial e política transparente |
| SEC-QA-12 | Pentest API e binário | Achados priorizados; nenhum crítico explorável no release |

## Fontes primárias consultadas em 19/10/2026

- [Play Integrity — visão geral](https://developer.android.com/google/play/integrity/overview): sinais complementares, rollout e operação.
- [Requisições padrão](https://developer.android.com/google/play/integrity/standard): vínculo por requestHash e verificação no backend.
- [Vereditos](https://developer.android.com/google/play/integrity/verdicts): licença e integridade da instalação.
- [Apple — validar apps que acessam servidor](https://developer.apple.com/documentation/devicecheck/validating-apps-that-connect-to-your-server): cadeia, challenge e contador de App Attest.
- [Apple AppTransaction](https://developer.apple.com/documentation/storekit/apptransaction): compra do aplicativo assinada.
- [Unity — managed stripping](https://docs.unity.com/en-us/engine/6000.0/manual/scripting/compilation-and-code-reload/script-compilation/managed-code-stripping): remoção e preservação de código.
- [OWASP MASVS](https://mas.owasp.org/MASVS/): controles de armazenamento, criptografia, autenticação, rede, plataforma, código, resiliência e privacidade.

Esta arquitetura é uma proposta aplicada a essas interfaces. Os prazos, gates, IDs e parâmetros são decisões do projeto, não recomendações atribuídas aos fornecedores. A matriz MASVS serve de referência, sem declaração de certificação ou conformidade integral. Reconsultar documentação antes de fixar SDKs e publicar.
