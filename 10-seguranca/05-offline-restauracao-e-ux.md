# Offline, restauração e experiência do comprador

## Máquina de estados

| Estado | Entrada | Jogo / ação |
|---|---|---|
| Precisa ativar | Sem licença verificável | Menu, configurações, ajuda, restaurar e loja; explicar conexão inicial |
| Validando | Requisição em curso | UI cancelável; se cache válido já existe, jogar sem esperar |
| Válida | Assinatura/vínculos/prazo válidos | Campanha e replay livres; renovar após 24 h se online |
| Tolerância | 30–37 dias desde emissão | Campanha livre; aviso discreto no menu |
| Precisa renovar | Após `offline_until` | Preservar save; renovar antes da próxima sessão |
| Relógio em revisão | Relógio local anterior à âncora além de 5 min | Pedir rede; não acusar fraude nem apagar dados |
| Revisão de compra | Prova negativa confirmada | Suporte/restaurar; nenhuma emissão sem prova |
| Serviço indisponível | Rede/loja fora do ar | Cache dentro do prazo continua; sem licença anterior oferecer nova tentativa |

Rótulos claros: “Conecte-se para confirmar sua compra”; “Sua aventura está salva neste aparelho”; “Não conseguimos confirmar agora. Tente novamente”; “Use a conta da loja que comprou o jogo”. Não mostrar códigos criptográficos ao jogador. Código de diagnóstico pode ser copiado voluntariamente para suporte.

## Retomada e suspensão mobile

Pausar o relógio da jogabilidade ao perder foco; prazo de licença continua em UTC. Renovação em background não congela física. Expiração durante uma fase permite chegar ao checkpoint e salvar antes de solicitar revalidação. `LicensePolicy` não aplica punição de jogabilidade; gate existe apenas ao iniciar/retomar sessão ou conteúdo protegido. Voltar da loja reavalia a compra sem reiniciar processo.

## Save e reinstalação

Save não é licença. Corrupção da licença não apaga save. Copiar save local entre aparelhos não transfere compra; compra deve ser restaurada. Inicialmente não há sincronização entre plataformas. Desinstalação pode remover o progresso conforme sistema e backup; o jogo deve informar isso na ajuda, e o fluxo de exportação/importação de save recebe schema, limites e validação de segurança.

## Encerramento do serviço

Antes de encerrar vendas/serviço, distribuir atualização de preservação que remova a renovação periódica para compras verificadas e permita campanha offline. Comunicar data, manter janela de atualização e suporte; não depender de um site que desaparece antes da atualização. Esse compromisso limita lock-in e deve entrar no custo operacional. A atualização futura requer release real assinado; não há chave mestra pública ou bypass secreto no pacote.

## Aprovação de política

Prazos de 30+7 dias são baseline V3 para implementar e testar, não regra de loja nem garantia jurídica. Validar restauração familiar e comportamento de reembolso em cada loja conforme os mecanismos oficialmente disponíveis. Não inferir identidade de conta Google ou data de compra a partir do selo de integridade.
