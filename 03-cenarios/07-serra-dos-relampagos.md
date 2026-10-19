# Capítulo 07 · Serra dos Relâmpagos

ID `LVL-07` · Ato 2 · Tempo-alvo 35 min, sem tempo de colecionismo · Baseline de implementação V3, parâmetros sujeitos a playtest.

## Contrato de entrada e saída

Entrada: LVL_06_COMPLETE persistida. Saída somente após `BOSS_07_DEFEATED` e objetivo central confirmado. Herói ativo começa em solo seguro; mascotes indisponíveis usam retorno automático, nunca bloqueiam missão.

Mecânica: raio prenunciado por círculo, áudio e contagem; Zé conduz somente em placas próprias.

## Percurso implementável

| Seção | Minutos | Acontecimento | Desafio e resposta | Flag persistente |
|---|---|---|---|---|
| Passarela em silêncio | 0–5 | Trabalhadores reconhecem Zé pelo código do crachá e baixam a ponte. | Mara entrega provas; travessia curta sem dano enquanto a conversa conclui. | `ZE_RECONHECIDO` |
| Colunas de descarga | 5–13 | Trovico revela onde o próximo raio atingirá a passarela. | Aprender ciclo 2 s de aviso/1 s ativo/3 s descanso. | `ATERRAMENTO_1` |
| Confissão no abrigo | 13–17 | Zé admite que o desvio tinha sua autorização e que preferiu não investigar. | Interação obrigatória curta ou skip com mesma flag, sem escolha moral falsa. | `ZE_CONFESSED` |
| Antena no temporal | 17–28 | Zé fica no aterramento enquanto Mara cruza o vão. O metal começa a ceder. | Três condutores e laço; assistente estende janela, não remove o objetivo. | `ANTENA_ISOLADA` |
| General | 28–33 | O General desloca hastes, tentando deixar o cume sem abrigo. | Aterrar descarga e puxar bobina durante resfriamento. | `BOSS_07_DEFEATED` |
| Uma ponte consertada | 33–35 | Zé repara a passarela antes de partir; o trabalhador abre caminho sem esquecer a dívida. | Salvar mapa e estado da ponte. | `LVL_07_COMPLETE` |

## Checkpoints e retomada

`CP-07-01` na entrada; `CP-07-02` após seção 2; `CP-07-03` após seção 4; `CP-07-BOSS` antes da arena. No capítulo 10 há `CP-10-RESGATE` depois de estabilizar o núcleo. Mortes revertem mecanismos locais da seção, preservam flags de objetivos concluídos, Atlas e coletáveis únicos. Puzzle incompleto volta ao estado inicial daquele puzzle. Nunca reaplicar dano durante o blend de retorno; 1,2 s de invulnerabilidade ao recuperar controle.

## Arena e confronto

Cume de 32 m, três hastes de aterramento com círculos de refúgio. Objetivo: desviar três descargas às hastes e desconectar as bobinas do General. A ficha `05-game-design/bosses/BOSS-07.md` contém temporização e transições. Arena só fecha após o protagonista chegar à zona segura; mascote ou herói necessário é disponibilizado por regra de capítulo. Sem consumível obrigatório.

## Revelação e gancho

Zé confessa que autorizou o desvio de água e ignorou um aviso. O mapa atmosférico abre rota para o observatório. Segredos ampliam contexto e nunca bloqueiam essa revelação ou final.

## Arte, som e montagem

Plano de jogo ocupa faixa XY com Z fixo. Fundo distante em 3 camadas, props de primeiro plano fora da faixa de aterrissagem e pontos interativos com forma/som próprios. Usar kit modular do bioma; iluminação bakeada e FX ajustáveis. SFX de preparação do chefe prevalece sobre música. Pouso seguro e dano não dependem da cor do material. Asset de confronto: `09-assets/personagens/general-para-raios.png`.

## Casos de aceite

Testar capítulo do início sem opcionais; morte em cada seção; fechamento do app após cada flag; replay sem duplicar recompensa; cutscenes assistidas e puladas; mascote chamado fora do alcance; 30/60 FPS; controles canhotos; assistências ativas. Saída só completa uma vez. Não há teleporte para coordenadas que ainda não carregaram.
