# Incidentes e recuperação

| Evento | Contenção imediata | Recuperação e critério |
|---|---|---|
| Loja ou rede indisponível | Circuit breaker e uso de cache válido | Restaurar fornecedor; medir sucesso; sem estender prazo local |
| Chave de licença comprometida | Desabilitar uso no KMS, congelar emissão afetada, auditar concessões | Rotacionar e distribuir confiança; reconhecer limite offline |
| Falso bloqueio em massa | Suspender rollout e regra remota problemática | Reproduzir em dispositivo legítimo; teste de restauração e retorno gradual |
| Serviço público exposto incorretamente | Restringir gateway, revogar credenciais vazadas, preservar logs | Revisão de autorização e teste externo antes de reabrir |
| Bundle corrompido | Parar promoção do catálogo | Publicar manifesto assinado para versão íntegra compatível |
| Banco perdido | Acionar plano de restore, API indisponível sem corromper clientes | Restaurar backup/WAL em rede isolada e validar concessões |
| Save corrompido | Bloquear sobrescrita e tentar backup | Retomar snapshot validado; diagnóstico sem conteúdo privado |

Responsáveis de função: Security Lead coordena, Operação contém, Backend Lead recupera, QA valida comprador, responsável de produto comunica. Nomes e escalas reais são cadastrados pelo proprietário ao montar a equipe. Registrar UTC, impacto, correlação, hipótese, ação e evidência; não fazer anúncio de causa antes da investigação.

Simulação antes de lançamento: perder chave de teste, derrubar fornecedor, restaurar banco e bloquear certificado errado. Meta de atendimento de incidente crítico: triagem em 1 h durante janela de operação contratada, sem prometer cobertura 24/7 inexistente. Fechar com revisão de causa, teste de regressão e ação datada. Encerramento definitivo segue a atualização de preservação descrita na política offline.
