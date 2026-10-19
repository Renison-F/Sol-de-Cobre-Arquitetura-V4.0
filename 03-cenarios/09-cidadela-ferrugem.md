# Capítulo 09 · Cidadela Ferrugem

ID `LVL-09` · Ato 3 · Tempo-alvo 42 min, sem tempo de colecionismo · Baseline de implementação V3, parâmetros sujeitos a playtest.

## Contrato de entrada e saída

Entrada: LVL_08_COMPLETE persistida. Saída somente após `BOSS_09_DEFEATED` e objetivo central confirmado. Herói ativo começa em solo seguro; mascotes indisponíveis usam retorno automático, nunca bloqueiam missão.

Mecânica: quatro tarefas sequenciais com mascotes e troca segura, sem exigir multiplayer.

## Percurso implementável

| Seção | Minutos | Acontecimento | Desafio e resposta | Flag persistente |
|---|---|---|---|---|
| Cidade que ainda bebe | 0–5 | Há água nas fontes da capital e filas de trabalhadores no turno da noite. | Rota de infiltração evita civis; mostrar objetivo de preservação. | `CIDADE_ENTRADA` |
| Canal da terra | 5–13 | Faísca entra no conduto fino; Mara prende o elevador que impediria a saída. | Laço e escavação; primeiro console persiste ao concluir. | `CANAL_SEGURANCA_1` |
| Canal da força | 13–21 | Zé empurra o gerador de reserva para não apagar o hospital da cidade. | Peso e aterramento; puzzle sem cronômetro. | `CANAL_SEGURANCA_2` |
| Canal do céu | 21–29 | Trovico cruza hélices paradas para destravar a terceira torre. | Rota aérea com linha de visão; abortar comando retorna mascote seguro. | `CANAL_SEGURANCA_3` |
| Madame | 29–38 | Madame segue ordens até ver o protocolo que o Barão omitiu. | Desarmar formação e apresentar prova após terceira janela. | `BOSS_09_DEFEATED` |
| Quarto canal | 38–42 | Madame abre o comando central e assume a evacuação dos operários. | Luna confirma quatro canais, snapshot e subida. | `LVL_09_COMPLETE` |

## Checkpoints e retomada

`CP-09-01` na entrada; `CP-09-02` após seção 2; `CP-09-03` após seção 4; `CP-09-BOSS` antes da arena. No capítulo 10 há `CP-10-RESGATE` depois de estabilizar o núcleo. Mortes revertem mecanismos locais da seção, preservam flags de objetivos concluídos, Atlas e coletáveis únicos. Puzzle incompleto volta ao estado inicial daquele puzzle. Nunca reaplicar dano durante o blend de retorno; 1,2 s de invulnerabilidade ao recuperar controle.

## Arena e confronto

Pátio industrial de 32 m, quatro consoles protegidos, duas passarelas altas. Objetivo: quebrar formação de drones sem colapsar o sistema e liberar comando de segurança. A ficha `05-game-design/bosses/BOSS-09.md` contém temporização e transições. Arena só fecha após o protagonista chegar à zona segura; mascote ou herói necessário é disponibilizado por regra de capítulo. Sem consumível obrigatório.

## Revelação e gancho

A própria capital perderá energia se o núcleo parar antes da abertura dos canais. Quatro canais abertos tornam possível subir ao núcleo com segurança. Segredos ampliam contexto e nunca bloqueiam essa revelação ou final.

## Arte, som e montagem

Plano de jogo ocupa faixa XY com Z fixo. Fundo distante em 3 camadas, props de primeiro plano fora da faixa de aterrissagem e pontos interativos com forma/som próprios. Usar kit modular do bioma; iluminação bakeada e FX ajustáveis. SFX de preparação do chefe prevalece sobre música. Pouso seguro e dano não dependem da cor do material. Asset de confronto: `09-assets/personagens/madame-engrenagem.png`.

## Casos de aceite

Testar capítulo do início sem opcionais; morte em cada seção; fechamento do app após cada flag; replay sem duplicar recompensa; cutscenes assistidas e puladas; mascote chamado fora do alcance; 30/60 FPS; controles canhotos; assistências ativas. Saída só completa uma vez. Não há teleporte para coordenadas que ainda não carregaram.

A última etapa emite `CANAL_SEGURANCA_4`, `LVL_09_COMPLETE` e `M09_COMPLETE` na mesma transação de snapshot. O quarto canal é principal e não depende de opcional.
