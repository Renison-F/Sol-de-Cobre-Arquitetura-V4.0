# Capítulo 10 · Coração da Tempestade

ID `LVL-10` · Ato 3 · Tempo-alvo 47 min, sem tempo de colecionismo · Baseline de implementação V3, parâmetros sujeitos a playtest.

## Contrato de entrada e saída

Entrada: LVL_09_COMPLETE persistida. Saída somente após `BOSS_10_DEFEATED` e objetivo central confirmado. Herói ativo começa em solo seguro; mascotes indisponíveis usam retorno automático, nunca bloqueiam missão.

Mecânica: síntese de laço, impacto, escavação e reconhecimento; nenhuma habilidade nova.

## Percurso implementável

| Seção | Minutos | Acontecimento | Desafio e resposta | Flag persistente |
|---|---|---|---|---|
| Ascensão | 0–6 | A água agora corre nos quatro canais; o anel de Isadora começa a abrir. | Confirmar flags de segurança; se faltar algo redirecionar ao console da Cidadela. | `NUCLEO_ACESSO` |
| A proposta do Barão | 6–11 | Ele oferece água para a estação em troca de manter o monopólio. Mara recusa. | Conversa com skip equivalente; nenhum falso final vendido como escolha. | `BARAO_CONFRONTED` |
| Blindagem | 11–23 | Zé descarrega bobinas, Mara remove três placas sem atingir a reserva. | Três janelas alternadas com chão sempre acessível. | `NUCLEO_BLINDAGEM_OFF` |
| Pulso da tempestade | 23–35 | Faísca solta válvula presa, Trovico orienta a rota entre anéis. | Três reguladores com save de transição antes da nova arena. | `NUCLEO_ESTAVEL` |
| Resgate | 35–43 | O passadiço cede. Mara prende o laço em Isadora e depois no Barão; Zé sustenta o cabo. | Sequência de 90 s ajustável; falha retorna ao começo do resgate já sem refazer chefe. | `ISADORA_RESCUED` |
| Primeira chuva | 43–47 | A torre devolve as reservas; Madame coordena reparos e o Barão entrega o controle à comunidade. | Epílogo e créditos puláveis; liberar seleção de capítulos e replay. | `LVL_10_COMPLETE` |

## Checkpoints e retomada

`CP-10-01` na entrada; `CP-10-02` após seção 2; `CP-10-03` após seção 4; `CP-10-BOSS` antes da arena. No capítulo 10 há `CP-10-RESGATE` depois de estabilizar o núcleo. Mortes revertem mecanismos locais da seção, preservam flags de objetivos concluídos, Atlas e coletáveis únicos. Puzzle incompleto volta ao estado inicial daquele puzzle. Nunca reaplicar dano durante o blend de retorno; 1,2 s de invulnerabilidade ao recuperar controle.

## Arena e confronto

Núcleo com três anéis de 34 m úteis; plataformas de resgate ligadas por cabos. Objetivo: desarmar blindagem, estabilizar núcleo e retirar Isadora e Barão antes da redistribuição. A ficha `05-game-design/bosses/BOSS-10.md` contém temporização e transições. Arena só fecha após o protagonista chegar à zona segura; mascote ou herói necessário é disponibilizado por regra de capítulo. Sem consumível obrigatório.

## Revelação e gancho

Barão aceita perder o comando para impedir que a reserva se rompa. Mara reencontra isadora fisicamente, chuva volta a todas as regiões, companhia perde monopólio. Segredos ampliam contexto e nunca bloqueiam essa revelação ou final.

## Arte, som e montagem

Plano de jogo ocupa faixa XY com Z fixo. Fundo distante em 3 camadas, props de primeiro plano fora da faixa de aterrissagem e pontos interativos com forma/som próprios. Usar kit modular do bioma; iluminação bakeada e FX ajustáveis. SFX de preparação do chefe prevalece sobre música. Pouso seguro e dano não dependem da cor do material. Asset de confronto: `09-assets/personagens/barao-ferrugem.png`.

## Casos de aceite

Testar capítulo do início sem opcionais; morte em cada seção; fechamento do app após cada flag; replay sem duplicar recompensa; cutscenes assistidas e puladas; mascote chamado fora do alcance; 30/60 FPS; controles canhotos; assistências ativas. Saída só completa uma vez. Não há teleporte para coordenadas que ainda não carregaram.

Entrada da arena exige `CANAL_SEGURANCA_1`, `CANAL_SEGURANCA_2`, `CANAL_SEGURANCA_3` e `CANAL_SEGURANCA_4` além de `LVL_09_COMPLETE`. Save migrado sem a quarta flag retorna ao console final da Cidadela com instrução clara; nunca fica preso diante de portão sem ação.
