# Backend de licenças — implantação alvo

## Serviços e dados

API stateless em região próxima do público inicial, gateway TLS 1.2/1.3, PostgreSQL com backups, Redis para nonces/limites e KMS para assinatura. Uma aplicação modular é suficiente inicialmente; não criar microserviços sem necessidade. Módulos: Enrollment, PlatformVerification, Entitlement, Signing, Audit e Support. Identidades de serviço têm escopo mínimo, segredo em cofre, nenhum segredo no Git, no portal ou no Unity.

## Requisição

Gateway limita corpo a 64 KiB; campos de prova até 48 KiB; timeouts externos de 10 s, orçamento total 20 s. Falha transitória: 503 + retry-after. Ativação: até 5 tentativas por instalação a cada 10 min e limite adicional de rede dimensionado para NAT; renovação 10/min com cache e idempotência. Esses limites iniciais são metas configuráveis, não resultados medidos. Nenhum rate limit por IP deve sozinho banir conta ou invalidar licença.

Nenhum endpoint público aceita `StoreResult`, `licensed`, prazo de concessão ou chave de assinatura. Corpo desconhecido é rejeitado. HTTP chama adaptador oficial e deriva a instalação autenticada; o resultado interno é produzido só pelo adaptador. O demo do ZIP não oferece servidor HTTP de licença justamente para evitar falso serviço comercial.

## Transações

Enrollment associa impressão de chave uma só vez após prova válida. Licença: adquirir idempotência, validar prova, reservar/consumir desafio e persistir concessão pendente numa transação; KMS assina payload imutável; finalizar com compare-and-set; repetição recupera o mesmo registro. Evitar chamar fornecedor externo segurando lock de banco por dezenas de segundos. Usar estados Pending/Issued/Failed e outbox com worker idempotente.

Assertion iOS: atualizar contador apenas se novo > antigo, de forma atômica. Evento autenticado de loja usa chave única `provider+event_id`, impede dupla revogação e aceita entrega fora de ordem por versão/tempo verificado. Notificação desconhecida não altera licença.

## Administração

Console de suporte separado da API de jogo. OIDC, MFA, papéis Reader/Support/Security/Release; Support não lê recibos nem assina prazo arbitrário. Reativação exige prova de compra; exceção de atendimento exige ticket, duração limitada, auditoria e autorização operacional. Não incluir credenciais de teste em release.

## Disponibilidade e custo

Metas iniciais: SLO 99,9% mensal para API, p95 próprio abaixo de 300 ms excluindo lojas e KMS; ativação completa p95 alvo 8 s, a medir. Backup diário + WAL, RPO alvo 15 min, RTO alvo 4 h; teste trimestral de restauração. Dimensionar custos pela quantidade de instalações, ativações, renovações e cotas verificadas em cada fornecedor, sem inventar orçamento de nuvem.

## Telemetria

Métricas: sucesso por plataforma/versão, latência, motivo sanitizado, taxa de licença em tolerância, restauração, nonce repetido, 429 e incidentes. Nunca logar prova, token, chave, save completo ou e-mail da loja. Alertar aumento de falhas e falsos bloqueios antes de endurecer política. Logs têm correlation ID aleatório, retenção curta e acesso auditado.
