# Telas e estados de interface do jogo

| Tela | Conteúdo e ação principal | Erro/retorno | Aceite |
|---|---|---|---|
| Inicialização | Marca breve, progresso real de recursos | Retry/cancelamento se recurso falha | Não esconder download infinito |
| Ativação inicial | Confirmar compra, restaurar e abrir loja | Rede indisponível, compra inconclusiva | Não cobrar de novo automaticamente |
| Menu principal | Continuar, novo jogo, capítulos, Atlas, opções | Save inválido leva a recuperação | Continuar mostra slot e capítulo |
| Slots | Três slots, data, capítulo, aviso ao substituir | Cancelar preserva slot | Sobrescrita exige confirmação concreta |
| Seleção de capítulo | Dez cartões, bloqueio por progressão | Capítulo não instalado informa tamanho | Replay não apaga campanha |
| HUD | Vida, próximo objetivo curto, touch contextual | Dica sem cobrir salto | Legível 720p, safe area |
| Pausa | Retomar, reiniciar checkpoint, opções, menu | Reiniciar mostra perda local prevista | Congela simulação, mantém licença UTC |
| Opções | Mix, legendas, assistência, flashes e controles | Restaurar defaults com preview | Tudo persiste por perfil |
| Atlas | Coletáveis, pistas, elenco e tutorial revisto | Conteúdo ainda não encontrado sinalizado | Sem spoiler involuntário do final |
| Recuperação de save | Arquivo atual inválido, backup disponível, exportar | Não resetar sem escolha do usuário | Preserva originais |
| Licença a renovar | Conectar/restaurar, diagnóstico opcional | Retry com backoff | Progresso preservado; não interrompe salto |
| Créditos e epílogo | Equipe, atribuições, música e skip | Volta ao menu e replay | Campanha concluída persiste antes de sair |

Fluxo do comprador: instalação → ativação → menu → slot → capítulo. Depois da primeira ativação, cache válido permite ir direto ao menu. Menus suportam touch/gamepad/teclado, foco visível e voltar consistente. Não expor requestHash, jti, KMS ou stack trace nas telas do jogador.

Loading real usa fases conhecidas e texto de status, não porcentagem inventada. Modal de erro tem título claro, uma ação principal e uma saída; mensagens não culpam o usuário. Tutorial é contextual e reapresentável no Atlas. Preferência de acessibilidade deve estar disponível antes de jogar.
