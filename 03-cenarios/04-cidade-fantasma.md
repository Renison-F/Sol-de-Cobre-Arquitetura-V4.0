# Capítulo 04 · Cidade Fantasma

ID `LVL-04` · Ato 2 · Tempo-alvo 29 min, sem tempo de colecionismo · Baseline de implementação V3, parâmetros sujeitos a playtest.

## Contrato de entrada e saída

Entrada: LVL_03_COMPLETE persistida. Saída somente após `BOSS_04_DEFEATED` e objetivo central confirmado. Herói ativo começa em solo seguro; mascotes indisponíveis usam retorno automático, nunca bloqueiam missão.

Mecânica: Trovico marca caminho alto; projeção não é plataforma sem borda sólida.

## Percurso implementável

| Seção | Minutos | Acontecimento | Desafio e resposta | Flag persistente |
|---|---|---|---|---|
| Rua de areia | 0–4 | O sino toca sozinho; uma luz repete passos que ninguém mais dá. | Caminhar entre dois lampiões e ler o contorno real das plataformas. | `SINO_ATIVO` |
| O batedor retorna | 4–9 | Trovico pousa no chapéu de Zé trazendo um parafuso do transmissor. | Comando aéreo em alvos com alcance 8 m e linha de visão. | `TROVICO_JOINED` |
| Evacuação em ecos | 9–17 | As projeções mostram famílias entrando no trem, não fantasmas hostis. | Acender três registros; cruzar duas sacadas com janela de luz de 10 s. | `EVACUACAO_VISTA` |
| Transmissor na torre | 17–23 | Luna ouve o início de uma fita interrompida e reconhece a frequência de Isadora. | Zé aterra torre, Mara sobe, Trovico destrava o suporte. | `TRANSMISSOR_LOCALIZADO` |
| Maquinista | 23–27 | O exoesqueleto automatizado repete o último protocolo de evacuação. | Iluminar cabos e retirar três travas sem ferir humanos. | `BOSS_04_DEFEATED` |
| Bilhete de embarque | 27–29 | O transmissor anuncia um arquivo protegido no próximo trem. | Salvar e partir ao desfiladeiro ferroviário. | `LVL_04_COMPLETE` |

## Checkpoints e retomada

`CP-04-01` na entrada; `CP-04-02` após seção 2; `CP-04-03` após seção 4; `CP-04-BOSS` antes da arena. No capítulo 10 há `CP-10-RESGATE` depois de estabilizar o núcleo. Mortes revertem mecanismos locais da seção, preservam flags de objetivos concluídos, Atlas e coletáveis únicos. Puzzle incompleto volta ao estado inicial daquele puzzle. Nunca reaplicar dano durante o blend de retorno; 1,2 s de invulnerabilidade ao recuperar controle.

## Arena e confronto

Praça de 26 m com três lampiões de memória e sacadas a 3 m. Objetivo: acender os três lampiões para revelar cabos do exoesqueleto e soltá-los. A ficha `05-game-design/bosses/BOSS-04.md` contém temporização e transições. Arena só fecha após o protagonista chegar à zona segura; mascote ou herói necessário é disponibilizado por regra de capítulo. Sem consumível obrigatório.

## Revelação e gancho

A vila foi evacuada para alimentar o primeiro teste climático. Transmissor aponta para um cofre no expresso de cobre. Segredos ampliam contexto e nunca bloqueiam essa revelação ou final.

## Arte, som e montagem

Plano de jogo ocupa faixa XY com Z fixo. Fundo distante em 3 camadas, props de primeiro plano fora da faixa de aterrissagem e pontos interativos com forma/som próprios. Usar kit modular do bioma; iluminação bakeada e FX ajustáveis. SFX de preparação do chefe prevalece sobre música. Pouso seguro e dano não dependem da cor do material. Asset de confronto: `09-assets/personagens/maquinista-sem-rosto.png`.

## Casos de aceite

Testar capítulo do início sem opcionais; morte em cada seção; fechamento do app após cada flag; replay sem duplicar recompensa; cutscenes assistidas e puladas; mascote chamado fora do alcance; 30/60 FPS; controles canhotos; assistências ativas. Saída só completa uma vez. Não há teleporte para coordenadas que ainda não carregaram.
