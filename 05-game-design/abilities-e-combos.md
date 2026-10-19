# Habilidades e composição de desafios

| ID | Dono | Entrada | Resultado | Limites / falha recuperável |
|---|---|---|---|---|
| AB-LACO | Mara | Pressionar habilidade com alvo selecionado | Prende argola até 7 m; balanço usa cabo com limite | Sem alvo, animação curta e nenhum gasto; soltar cancela em segurança |
| AB-IMPACTO | Zé | Habilidade em solo | Onda 2 m em objetos marcados | Cooldown 1 s; não quebra geometria obrigatória fora da ordem |
| AB-TERRA | Zé | Segurar interação em placa | Aterra carga até 2 s | Sai da placa/cancela: zera carga local, sem dano surpresa |
| AB-TUNEL | Faísca | Contexto de terra até 6 m | Percorre canal e ativa objeto | Ação 1,5 s; retorna em 2 s; sem rota válida oferece dica |
| AB-VOO | Trovico | Contexto visível até 8 m | Reconhece/aciona interruptor alto | Linha de visão e cooldown 3 s; não transporta herói indefinidamente |

Prioridade de seleção: objeto de missão válido no cone → alvo central mais próximo → desempate por ID estável. HUD mostra alvo antes do comando. Não trocar alvo no meio de animação comprometida.

Combos são sequências de cooperação, não códigos de luta: Mara segura contrapeso para Zé; Faísca libera válvula para Luna; Trovico marca arco para Mara; Zé aterra e Mara puxa. Cada etapa confirma evento próprio e reset tem estado inicial definido. Troca de herói indisponível no ar, na hit reaction ou interação crítica; input pode ser enfileirado por 0,3 s e executado no próximo solo seguro.

Laço também pode prender argola exposta de inimigo após preparação/ataque falho, aplicando uma unidade de integridade. Alvos têm símbolo do mesmo nó do tutorial. Enquanto blindado, laço dá feedback sem dano; esperar a janela é a regra.
