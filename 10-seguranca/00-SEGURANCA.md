# Segurança comercial e proteção do jogador

**Decisão V3 — 19/10/2026.** Jogo premium, compra do aplicativo inteiro nas lojas, campanha individual. Sem anúncios, moeda comprável, ranking competitivo ou exigência de cadastro próprio na primeira versão. A assinatura de licença protege o acesso; a campanha não é executada no servidor.

## Objetivo e limite

Aumentar o custo de redistribuição adulterada, rejeitar provas falsas nos serviços e proteger compras, saves e infraestrutura. Nenhum binário executado no aparelho do usuário pode ter promessa técnica de ser impossível de crackear. Um atacante pode substituir a verificação local e extrair conteúdo após a decodificação. Ofuscação e criptografia de assets não corrigem essa fronteira. Segurança será medida por testes, incidentes, falsos bloqueios e tempo de resposta.

## Fronteiras de confiança

| Zona | Pode fazer | Não pode fazer |
|---|---|---|
| Cliente Unity e armazenamento local | Executar campanha, guardar cache assinado, apresentar prova nativa | Declarar que pagou ou alterar titularidade no backend |
| Loja e serviço de atestação | Fornecer prova verificável de compra ou integridade | Ser substituído por um booleano vindo do cliente |
| Adaptador autenticado de loja | Validar cadeia, aplicação, ambiente, vínculo e atualidade | Aceitar payload decodificado sem validação criptográfica |
| Serviço de licenças | Aplicar política, consumir nonce, pedir assinatura ao KMS | Exportar chave privada para o cliente |
| CDN | Servir versão de conteúdo imutável com manifesto assinado | Executar scripts arbitrários recebidos do portal |
| Portal documental local | Ler documentação da pasta extraída | Agir como backend comercial ou custodiar segredos |

## Fluxo normal

1. Instalação cria identificador aleatório e par de chaves protegido por Keystore/Keychain; não usar IMEI ou identificador publicitário.
2. Bootstrap obtém desafio de 120 s. Rate limit e registro da chave são feitos antes de emitir autorização de conteúdo.
3. Native bridge obtém prova de loja e de integridade vinculadas ao desafio, instalação, chave pública e operação. O servidor autentica o vínculo antes de criar `StoreResult` interno.
4. Emissor consome desafio uma única vez e emite envelope assinado RSA-3072/SHA-256, validade normal de 30 dias e limite offline absoluto de 37 dias desde emissão.
5. Cliente verifica assinatura e todos os vínculos antes de liberar campanha. Renovação oportunista após 24 h quando houver rede, sem chamar atestação a cada frame ou fase.
6. Sem rede, licença válida ou em tolerância mantém campanha disponível; não há extensão local de validade. Após 37 dias, menu oferece reconexão/restauração e preserva os saves. Uma sessão já iniciada chega ao próximo checkpoint; o aviso não interrompe um salto.

Os prazos são decisão de produto V3, configuráveis somente por política assinada futura e sujeitos a teste com compradores. Primeira ativação requer rede. Isso altera a premissa da V2 de funcionamento inicial totalmente offline. A licença temporária exige manutenção contínua do serviço; o plano de encerramento precisa ser executado antes de desligá-lo.

## Evidência incluída

`security-reference/licensing.py` implementa assinatura, verificação, prazo, vínculos e desafio atômico em processo único, com testes reproduzíveis. `unity-project-template/Assets/_Game/Domain/Security/` define interfaces e política de prazo em C#. Não há adaptadores reais das lojas, KMS ou serviço público implantado. Nenhum endpoint aceita `licensed: true` do consumidor.

## Documentos operacionais

Ler modelo de ameaças, Android, iOS, protocolo, política offline, backend, build, conteúdo, privacidade, incidentes e plano de testes desta pasta. `11-contratos/` contém OpenAPI, SQL e schemas. `12-desenvolvimento/` liga os controles aos marcos de implementação.
