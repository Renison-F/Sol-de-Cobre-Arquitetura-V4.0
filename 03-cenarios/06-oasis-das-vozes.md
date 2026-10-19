# Capítulo 06 · Oásis das Vozes

ID `LVL-06` · Ato 2 · Tempo-alvo 30 min, sem tempo de colecionismo · Baseline de implementação V3, parâmetros sujeitos a playtest.

## Contrato de entrada e saída

Entrada: LVL_05_COMPLETE persistida. Saída somente após `BOSS_06_DEFEATED` e objetivo central confirmado. Herói ativo começa em solo seguro; mascotes indisponíveis usam retorno automático, nunca bloqueiam missão.

Mecânica: pressão em faixas com marcas e números; água é caminho, não recurso consumível.

## Percurso implementável

| Seção | Minutos | Acontecimento | Desafio e resposta | Flag persistente |
|---|---|---|---|---|
| Refúgio desconfiado | 0–4 | Habitantes recuam ao ver as bobinas de Zé. Ele decide guardá-las. | Explorar sem combate e mostrar prova dos registros. | `OASIS_ACESSO` |
| Jardim de pressão | 4–10 | Luna troca uma junta rompida e pede que Mara regule o primeiro tanque. | Válvula de três posições, indicador com números e forma. | `CANAL_1` |
| Raízes alagadas | 10–18 | Faísca alcança a tubulação fina sem arrancar as raízes. | Desviar água por rota alternativa; erro volta nível de tanque, não reinicia capítulo. | `CANAL_2` |
| Diário da nascente | 18–24 | Mara encontra instruções manuscritas e uma marca recente da mãe. | Último canal exige sequência 2-1-3; dica progressiva. | `CANAL_3` |
| Guardião | 24–28 | O sensor antigo confunde o condensador de Luna com equipamento da Companhia. | Equilibrar pressão e recalibrar três sensores. | `BOSS_06_DEFEATED` |
| A promessa de retorno | 28–30 | Zé entrega sua insígnia à comunidade e promete voltar com água para todas as vilas. | Salvar reparação; abrir serra e marca de confiança. | `LVL_06_COMPLETE` |

## Checkpoints e retomada

`CP-06-01` na entrada; `CP-06-02` após seção 2; `CP-06-03` após seção 4; `CP-06-BOSS` antes da arena. No capítulo 10 há `CP-10-RESGATE` depois de estabilizar o núcleo. Mortes revertem mecanismos locais da seção, preservam flags de objetivos concluídos, Atlas e coletáveis únicos. Puzzle incompleto volta ao estado inicial daquele puzzle. Nunca reaplicar dano durante o blend de retorno; 1,2 s de invulnerabilidade ao recuperar controle.

## Arena e confronto

Bacia de 30 m com três canais em alturas 0/1,5/3 m e ilhas seguras. Objetivo: equalizar três reservatórios, corrigir leitura do guardião e preservar comportas. A ficha `05-game-design/bosses/BOSS-06.md` contém temporização e transições. Arena só fecha após o protagonista chegar à zona segura; mascote ou herói necessário é disponibilizado por regra de capítulo. Sem consumível obrigatório.

## Revelação e gancho

Diários mostram que isadora segura pessoalmente a contenção do pulso. Com canais locais reparados, a serra fornece o aterramento necessário. Segredos ampliam contexto e nunca bloqueiam essa revelação ou final.

## Arte, som e montagem

Plano de jogo ocupa faixa XY com Z fixo. Fundo distante em 3 camadas, props de primeiro plano fora da faixa de aterrissagem e pontos interativos com forma/som próprios. Usar kit modular do bioma; iluminação bakeada e FX ajustáveis. SFX de preparação do chefe prevalece sobre música. Pouso seguro e dano não dependem da cor do material. Asset de confronto: `09-assets/personagens/guardiao-dos-canais.png`.

## Casos de aceite

Testar capítulo do início sem opcionais; morte em cada seção; fechamento do app após cada flag; replay sem duplicar recompensa; cutscenes assistidas e puladas; mascote chamado fora do alcance; 30/60 FPS; controles canhotos; assistências ativas. Saída só completa uma vez. Não há teleporte para coordenadas que ainda não carregaram.
