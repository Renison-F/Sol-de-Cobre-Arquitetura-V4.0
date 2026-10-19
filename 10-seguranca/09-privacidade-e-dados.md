# Privacidade, minimização e retenção

Produto inicialmente sem publicidade, analytics comportamental de terceiros ou chat. Dados necessários: instalação aleatória, chave pública/ID de atestação, concessões pseudônimas, versão do app, resultado sanitizado da loja e logs de segurança. O progresso permanece local. Coleta de diagnóstico detalhado é voluntária e revisável.

## Política técnica proposta

| Dado | Finalidade | Retenção alvo | Acesso |
|---|---|---|---|
| Desafio de ativação | Evitar replay | 120 s + metadado de auditoria sem prova | Serviço |
| Prova opaca de loja | Validação pontual | Memória; não persistir por padrão | Adaptador |
| Instalação/chave pública | Renovação e suporte | Vida ativa; limpar após 180 dias inativa salvo necessidade justificada | Serviço + segurança |
| Concessão/jti | Validade e revogação | Até 90 dias após término como baseline | Segurança |
| Logs sanitizados | Incidentes e disponibilidade | 30 dias | Operação autorizada |
| Exportação voluntária de diagnóstico | Atendimento | Até 30 dias após encerrar ticket | Suporte |

Retenções são proposta de produto, não afirmação de obrigação legal. Validar avisos, bases aplicáveis, direitos, público etário, país de operação, fornecedores e formulários de privacidade das lojas antes de publicar. Não incluir uma política jurídica fictícia com CNPJ ou contato inventado. A classificação etária precisa refletir conteúdo final, não só este roteiro.

Não coletar contatos, localização precisa, publicidade ID, microfone ou câmera. Notificações não são necessárias ao lançamento. Log não contém recibos, token ou texto de save. Pedidos de exclusão não viram remoção unilateral da compra da loja; explicar limites de backup local e reativação. Auditoria deve diferenciar pseudônimo, dado pessoal e anonimização real.
