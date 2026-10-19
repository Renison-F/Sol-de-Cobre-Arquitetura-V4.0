# Capítulo 05 · O Expresso de Cobre

ID `LVL-05` · Ato 2 · Tempo-alvo 32 min, sem tempo de colecionismo · Baseline de implementação V3, parâmetros sujeitos a playtest.

## Contrato de entrada e saída

Entrada: LVL_04_COMPLETE persistida. Saída somente após `BOSS_05_DEFEATED` e objetivo central confirmado. Herói ativo começa em solo seguro; mascotes indisponíveis usam retorno automático, nunca bloqueiam missão.

Mecânica: movimento relativo de trem simulado por cenário; polaridade com símbolos + e −.

## Percurso implementável

| Seção | Minutos | Acontecimento | Desafio e resposta | Flag persistente |
|---|---|---|---|---|
| Embarque de manutenção | 0–4 | Luna usa antiga credencial; Mara entra pelo teto quando o acesso é negado. | Introduzir vento lateral moderado e câmera em velocidade fixa. | `TREM_EMBARCADO` |
| Tetos e pantógrafos | 4–11 | Drones fecham corredores. Trovico marca intervalos entre arcos elétricos. | Mara usa laço nos tetos; Zé cruza por dentro com aterramento. | `VAGOES_CRUZADOS` |
| Cofre de voz | 11–17 | A chave da próxima rota está na caixa de Isadora. | Alinhar dois contatos com sinais opostos; erro reinicia só esse mecanismo. | `FITA_RECUPERADA` |
| Túnel à vista | 17–25 | O Barão manda desacoplar o vagão do arquivo. A ponte seguinte desaparece na neblina. | Fuga 120 s com assistente sem limite; falha retorna ao cofre já aberto. | `CHAVE_OBSERVATORIO` |
| Condutor | 25–30 | O autômato usa os vagões como um circuito. | Alternar vagão seguro, descarregar ímãs e puxar freio. | `BOSS_05_DEFEATED` |
| Voz na fita | 30–32 | Isadora ordena: primeiro os canais, depois o núcleo. | Salvar fita e chave; rampa segura até o ramal. | `LVL_05_COMPLETE` |

## Checkpoints e retomada

`CP-05-01` na entrada; `CP-05-02` após seção 2; `CP-05-03` após seção 4; `CP-05-BOSS` antes da arena. No capítulo 10 há `CP-10-RESGATE` depois de estabilizar o núcleo. Mortes revertem mecanismos locais da seção, preservam flags de objetivos concluídos, Atlas e coletáveis únicos. Puzzle incompleto volta ao estado inicial daquele puzzle. Nunca reaplicar dano durante o blend de retorno; 1,2 s de invulnerabilidade ao recuperar controle.

## Arena e confronto

Três vagões conectados, 32 m úteis, acoplamentos protegidos por guarda-corpo. Objetivo: aterrar dois ímãs e puxar o freio de emergência após três curtos controlados. A ficha `05-game-design/bosses/BOSS-05.md` contém temporização e transições. Arena só fecha após o protagonista chegar à zona segura; mascote ou herói necessário é disponibilizado por regra de capítulo. Sem consumível obrigatório.

## Revelação e gancho

A fita de isadora diz que abrir canais deve preceder a parada do núcleo. Salto de fuga leva ao ramal do oásis, trem para em faixa segura. Segredos ampliam contexto e nunca bloqueiam essa revelação ou final.

## Arte, som e montagem

Plano de jogo ocupa faixa XY com Z fixo. Fundo distante em 3 camadas, props de primeiro plano fora da faixa de aterrissagem e pontos interativos com forma/som próprios. Usar kit modular do bioma; iluminação bakeada e FX ajustáveis. SFX de preparação do chefe prevalece sobre música. Pouso seguro e dano não dependem da cor do material. Asset de confronto: `09-assets/personagens/condutor-relampago.png`.

## Casos de aceite

Testar capítulo do início sem opcionais; morte em cada seção; fechamento do app após cada flag; replay sem duplicar recompensa; cutscenes assistidas e puladas; mascote chamado fora do alcance; 30/60 FPS; controles canhotos; assistências ativas. Saída só completa uma vez. Não há teleporte para coordenadas que ainda não carregaram.
