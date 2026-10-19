# Capítulo 02 · Cânion dos Mil Ecos

ID `LVL-02` · Ato 1 · Tempo-alvo 28 min, sem tempo de colecionismo · Baseline de implementação V3, parâmetros sujeitos a playtest.

## Contrato de entrada e saída

Entrada: LVL_01_COMPLETE persistida. Saída somente após `BOSS_02_DEFEATED` e objetivo central confirmado. Herói ativo começa em solo seguro; mascotes indisponíveis usam retorno automático, nunca bloqueiam missão.

Mecânica: ecos revelam apoios, sempre com contorno e som; nunca apenas pela cor.

## Percurso implementável

| Seção | Minutos | Acontecimento | Desafio e resposta | Flag persistente |
|---|---|---|---|---|
| Ponte partida | 0–4 | Um apito vindo das pedras responde ao assobio de Mara. | Cruzar apoios visíveis e ativar primeiro sino. | `ECO_1` |
| Galeria de ressonância | 4–10 | Três sinos devolvem notas com formas distintas: círculo, traço, estrela. | Repetir sequência 1-3-2; ajuda destaca a próxima forma após duas falhas. | `ECO_2` |
| Duto suspenso | 10–17 | Uma tubulação esconde o leito do rio no alto do cânion. | Laço em apoios revelados por pulso; contorno permanece por 8 s. | `DUTO_VISTO` |
| Corrida de pedras | 17–22 | A Cascavel vibra o paredão. Mara corre entre quedas anunciadas por poeira. | Três ciclos de queda; faixa segura permanece em cada ciclo. | `ECO_3` |
| Cascavel | 22–26 | A serpente protege o encaixe de cristal que oculta a entrada da mina. | Atrair vibração para pilares, expor cauda e puxar. | `BOSS_02_DEFEATED` |
| Descida | 26–28 | O rio sumiu para dentro da montanha. Uma voz fanfarrona vem do elevador. | Snapshot e transição ao nível 3. | `LVL_02_COMPLETE` |

## Checkpoints e retomada

`CP-02-01` na entrada; `CP-02-02` após seção 2; `CP-02-03` após seção 4; `CP-02-BOSS` antes da arena. No capítulo 10 há `CP-10-RESGATE` depois de estabilizar o núcleo. Mortes revertem mecanismos locais da seção, preservam flags de objetivos concluídos, Atlas e coletáveis únicos. Puzzle incompleto volta ao estado inicial daquele puzzle. Nunca reaplicar dano durante o blend de retorno; 1,2 s de invulnerabilidade ao recuperar controle.

## Arena e confronto

Terraço de 30 m com três pilares ressonantes e dois refúgios altos. Objetivo: reproduzir três ecos para expor o chocalho e prendê-lo com laço. A ficha `05-game-design/bosses/BOSS-02.md` contém temporização e transições. Arena só fecha após o protagonista chegar à zona segura; mascote ou herói necessário é disponibilizado por regra de capítulo. Sem consumível obrigatório.

## Revelação e gancho

A água desce por um duto industrial que atravessa a montanha. Um elevador de minério leva mara ao turno de zé trovão. Segredos ampliam contexto e nunca bloqueiam essa revelação ou final.

## Arte, som e montagem

Plano de jogo ocupa faixa XY com Z fixo. Fundo distante em 3 camadas, props de primeiro plano fora da faixa de aterrissagem e pontos interativos com forma/som próprios. Usar kit modular do bioma; iluminação bakeada e FX ajustáveis. SFX de preparação do chefe prevalece sobre música. Pouso seguro e dano não dependem da cor do material. Asset de confronto: `09-assets/personagens/cascavel-de-silicio.png`.

## Casos de aceite

Testar capítulo do início sem opcionais; morte em cada seção; fechamento do app após cada flag; replay sem duplicar recompensa; cutscenes assistidas e puladas; mascote chamado fora do alcance; 30/60 FPS; controles canhotos; assistências ativas. Saída só completa uma vez. Não há teleporte para coordenadas que ainda não carregaram.
