# Capítulo 03 · Minas de Ferrugem

ID `LVL-03` · Ato 1 · Tempo-alvo 34 min, sem tempo de colecionismo · Baseline de implementação V3, parâmetros sujeitos a playtest.

## Contrato de entrada e saída

Entrada: LVL_02_COMPLETE persistida. Saída somente após `BOSS_03_DEFEATED` e objetivo central confirmado. Herói ativo começa em solo seguro; mascotes indisponíveis usam retorno automático, nunca bloqueiam missão.

Mecânica: alternância Mara/Zé em chão seguro, impacto e aterramento.

## Percurso implementável

| Seção | Minutos | Acontecimento | Desafio e resposta | Flag persistente |
|---|---|---|---|---|
| Turno interrompido | 0–5 | Zé segura uma viga enquanto Mara resgata trabalhadores sem nome. | Introduzir troca de herói, sem inimigos no primeiro uso. | `ZE_JOINED` |
| Peso e contrapeso | 5–11 | O elevador de Luna trava entre duas galerias. | Zé empurra bloco de 2 m; Mara prende contrapeso com laço. | `ELEVADOR_ESTAVEL` |
| Projetos de Luna | 11–18 | Luna recusa sair sem seu condensador: é o mapa da drenagem. | Mara desvia vapor; Faísca traz fusível do túnel. | `LUNA_RESCUED` |
| Canal de desvio | 18–26 | Zé percebe sua antiga assinatura no esquema. Esconde o papel por um instante. | Aterrar duas bobinas e fechar somente desvio industrial, mantendo drenagem segura. | `CONDENSADOR_RECUPERADO` |
| Britador | 26–32 | A máquina desperta no trilho do elevador. | Induzir broca, trocar em área segura e golpear radiador. | `BOSS_03_DEFEATED` |
| O custo do mapa | 32–34 | O mapa revela um cano até a Cidadela. Luna exige que ninguém desligue a água às cegas. | Salvar Luna aliada; abrir Cidade Fantasma. | `LVL_03_COMPLETE` |

## Checkpoints e retomada

`CP-03-01` na entrada; `CP-03-02` após seção 2; `CP-03-03` após seção 4; `CP-03-BOSS` antes da arena. No capítulo 10 há `CP-10-RESGATE` depois de estabilizar o núcleo. Mortes revertem mecanismos locais da seção, preservam flags de objetivos concluídos, Atlas e coletáveis únicos. Puzzle incompleto volta ao estado inicial daquele puzzle. Nunca reaplicar dano durante o blend de retorno; 1,2 s de invulnerabilidade ao recuperar controle.

## Arena e confronto

Plataforma de 28 m, dois trilhos de broca, três pilares substituíveis. Objetivo: desviar a broca para pilares e usar impacto de Zé no radiador. A ficha `05-game-design/bosses/BOSS-03.md` contém temporização e transições. Arena só fecha após o protagonista chegar à zona segura; mascote ou herói necessário é disponibilizado por regra de capítulo. Sem consumível obrigatório.

## Revelação e gancho

Luna demonstra que os dutos abastecem exclusivamente a capital do barão. Luna mantém a oficina móvel; grupo toma a estrada para a vila evacuada. Segredos ampliam contexto e nunca bloqueiam essa revelação ou final.

## Arte, som e montagem

Plano de jogo ocupa faixa XY com Z fixo. Fundo distante em 3 camadas, props de primeiro plano fora da faixa de aterrissagem e pontos interativos com forma/som próprios. Usar kit modular do bioma; iluminação bakeada e FX ajustáveis. SFX de preparação do chefe prevalece sobre música. Pouso seguro e dano não dependem da cor do material. Asset de confronto: `09-assets/personagens/britador-colosso.png`.

## Casos de aceite

Testar capítulo do início sem opcionais; morte em cada seção; fechamento do app após cada flag; replay sem duplicar recompensa; cutscenes assistidas e puladas; mascote chamado fora do alcance; 30/60 FPS; controles canhotos; assistências ativas. Saída só completa uma vez. Não há teleporte para coordenadas que ainda não carregaram.
