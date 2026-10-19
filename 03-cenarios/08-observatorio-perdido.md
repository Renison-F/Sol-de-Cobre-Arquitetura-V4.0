# Capítulo 08 · Observatório Perdido

ID `LVL-08` · Ato 3 · Tempo-alvo 40 min, sem tempo de colecionismo · Baseline de implementação V3, parâmetros sujeitos a playtest.

## Contrato de entrada e saída

Entrada: LVL_07_COMPLETE persistida. Saída somente após `BOSS_08_DEFEATED` e objetivo central confirmado. Herói ativo começa em solo seguro; mascotes indisponíveis usam retorno automático, nunca bloqueiam missão.

Mecânica: lentes mudam feixes e plataformas; gravidade episódica limitada a zonas explicitamente marcadas.

## Percurso implementável

| Seção | Minutos | Acontecimento | Desafio e resposta | Flag persistente |
|---|---|---|---|---|
| Porta do atlas | 0–5 | A bússola encaixa no mapa que Isadora desenhou para Mara anos antes. | Usar chave do trem e alinhar constelação simples. | `OBSERVATORIO_ABERTO` |
| Galeria de lentes | 5–15 | Feixes revelam apoios presos às órbitas. Luna limita o giro para evitar sobrecarga. | Girar espelhos em passos de 90 graus e percorrer plataformas. | `LENTES_ALINHADAS` |
| Anel suspenso | 15–23 | Uma faixa de gravidade permite um salto alongado, sem inverter controles. | Uma zona baixa segura e depois uma sobre abismo com retorno antes da zona. | `ANEL_CRUZADO` |
| Voz sem gravação | 23–29 | Mara pergunta algo que nenhuma gravação saberia. Isadora responde e o mapa ganha sentido. | Revelação incondicional; nenhum segredo opcional bloqueia resgate. | `ISADORA_CONTACTED` |
| Oráculo | 29–37 | O computador reproduz a primeira solução de Mara. Ela precisa mudar a ordem de acionamento. | Alternar três espelhos; prever só último padrão registrado, nunca input oculto. | `BOSS_08_DEFEATED` |
| Protocolo completo | 37–40 | Isadora entrega o protocolo de abertura gradual. O Barão interrompe o sinal. | Salvar protocolo principal e abrir Cidadela. | `LVL_08_COMPLETE` |

## Checkpoints e retomada

`CP-08-01` na entrada; `CP-08-02` após seção 2; `CP-08-03` após seção 4; `CP-08-BOSS` antes da arena. No capítulo 10 há `CP-10-RESGATE` depois de estabilizar o núcleo. Mortes revertem mecanismos locais da seção, preservam flags de objetivos concluídos, Atlas e coletáveis únicos. Puzzle incompleto volta ao estado inicial daquele puzzle. Nunca reaplicar dano durante o blend de retorno; 1,2 s de invulnerabilidade ao recuperar controle.

## Arena e confronto

Rotunda de 28 m com três espelhos giratórios e apoios presos ao plano lateral. Objetivo: alterar o padrão de três espelhos para saturar o cálculo do Oráculo. A ficha `05-game-design/bosses/BOSS-08.md` contém temporização e transições. Arena só fecha após o protagonista chegar à zona segura; mascote ou herói necessário é disponibilizado por regra de capítulo. Sem consumível obrigatório.

## Revelação e gancho

Isadora está viva no anel de contenção ligado ao núcleo por transmissão. Barão ameaça reservar toda a chuva à cidadela; canais de segurança são o próximo alvo. Segredos ampliam contexto e nunca bloqueiam essa revelação ou final.

## Arte, som e montagem

Plano de jogo ocupa faixa XY com Z fixo. Fundo distante em 3 camadas, props de primeiro plano fora da faixa de aterrissagem e pontos interativos com forma/som próprios. Usar kit modular do bioma; iluminação bakeada e FX ajustáveis. SFX de preparação do chefe prevalece sobre música. Pouso seguro e dano não dependem da cor do material. Asset de confronto: `09-assets/personagens/oraculo-de-latao.png`.

## Casos de aceite

Testar capítulo do início sem opcionais; morte em cada seção; fechamento do app após cada flag; replay sem duplicar recompensa; cutscenes assistidas e puladas; mascote chamado fora do alcance; 30/60 FPS; controles canhotos; assistências ativas. Saída só completa uma vez. Não há teleporte para coordenadas que ainda não carregaram.
