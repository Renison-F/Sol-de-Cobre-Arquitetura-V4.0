# Capítulo 01 · Estação Pôr do Sol

ID `LVL-01` · Ato 1 · Tempo-alvo 22 min, sem tempo de colecionismo · Baseline de implementação V3, parâmetros sujeitos a playtest.

## Contrato de entrada e saída

Entrada: nova campanha ou seleção de capítulo. Saída somente após `BOSS_01_DEFEATED` e objetivo central confirmado. Herói ativo começa em solo seguro; mascotes indisponíveis usam retorno automático, nunca bloqueiam missão.

Mecânica: laço em argolas com símbolo de nó; Faísca escava apenas terra marcada.

## Percurso implementável

| Seção | Minutos | Acontecimento | Desafio e resposta | Flag persistente |
|---|---|---|---|---|
| Poço vazio | 0–3 | A baldeada de Mara volta seca. Faísca segue um cano vibrando sob o bebedouro. | Andar, saltar dois degraus e examinar o poço; nenhum dano. | `POCO_EXAMINADO` |
| Telhados da estação | 3–7 | Um vagão de manutenção arranca levando a placa. Mara corre por toldos e caixas. | Ensinar laço com 3 argolas fixas, 2/3/4 m entre apoios. | `LACO_APRENDIDO` |
| Sob o depósito | 7–11 | Faísca encontra a derivação clandestina. O túnel leva ao interruptor do portão. | Comando contextual do mascote, duas rotas com saída comum. | `DESVIO_ENCONTRADO` |
| Passagem de carga | 11–16 | O Fiscal derruba uma barreira; o cargueiro começa a ganhar velocidade. | Sequência de três saltos; queda retorna ao início da seção. | `PATIO_ABERTO` |
| Fiscal de Trilhos | 16–20 | O Fiscal carimba “acesso negado” até o próprio mecanismo emperrar. | Desviar do braço, prendê-lo no bloco e abrir a traseira. | `BOSS_01_DEFEATED` |
| Placa e partida | 20–22 | Mara reconhece a marca da mãe. O trilho aponta para uma garganta de pedra. | Salvar e abrir acesso ao próximo capítulo. | `LVL_01_COMPLETE` |

## Checkpoints e retomada

`CP-01-01` na entrada; `CP-01-02` após seção 2; `CP-01-03` após seção 4; `CP-01-BOSS` antes da arena. No capítulo 10 há `CP-10-RESGATE` depois de estabilizar o núcleo. Mortes revertem mecanismos locais da seção, preservam flags de objetivos concluídos, Atlas e coletáveis únicos. Puzzle incompleto volta ao estado inicial daquele puzzle. Nunca reaplicar dano durante o blend de retorno; 1,2 s de invulnerabilidade ao recuperar controle.

## Arena e confronto

Pátio de cargas, 24 m de largura, três plataformas a 0/2/4 m. Objetivo: prender o carimbo em três blocos de carga e puxar a trava dorsal. A ficha `05-game-design/bosses/BOSS-01.md` contém temporização e transições. Arena só fecha após o protagonista chegar à zona segura; mascote ou herói necessário é disponibilizado por regra de capítulo. Sem consumível obrigatório.

## Revelação e gancho

Uma placa de isadora está presa no coletor de água da estação. Mara segue as coordenadas do cânion; a companhia fecha o desvio atrás dela. Segredos ampliam contexto e nunca bloqueiam essa revelação ou final.

## Arte, som e montagem

Plano de jogo ocupa faixa XY com Z fixo. Fundo distante em 3 camadas, props de primeiro plano fora da faixa de aterrissagem e pontos interativos com forma/som próprios. Usar kit modular do bioma; iluminação bakeada e FX ajustáveis. SFX de preparação do chefe prevalece sobre música. Pouso seguro e dano não dependem da cor do material. Asset de confronto: `09-assets/personagens/fiscal-de-trilhos.png`.

## Casos de aceite

Testar capítulo do início sem opcionais; morte em cada seção; fechamento do app após cada flag; replay sem duplicar recompensa; cutscenes assistidas e puladas; mascote chamado fora do alcance; 30/60 FPS; controles canhotos; assistências ativas. Saída só completa uma vez. Não há teleporte para coordenadas que ainda não carregaram.
