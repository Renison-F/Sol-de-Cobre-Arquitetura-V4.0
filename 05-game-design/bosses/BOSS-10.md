# BOSS-10 · Barão Ferrugem

Vinculado a LVL-10. Arte: `09-assets/personagens/barao-ferrugem.png`.

## Fantasia e vitória

Desarmar blindagem, estabilizar núcleo e retirar isadora e barão antes da redistribuição. O confronto usa 6 exposições de núcleo como contagem de integridade; um acerto válido por exposição, sem DPS ilimitado ou exigência de grind. Dano ao herói: uma unidade; vida padrão 5. Sem colisão de contato enquanto chefe está inativo. O Barão é resgatado; máquinas são desativadas sem violência gráfica.

Arena: núcleo com três anéis de 34 m úteis; plataformas de resgate ligadas por cabos. Cada posição ameaçada tem rota segura alcançável com os parâmetros do herói do capítulo.

## Ataques configuráveis

| Ação | Aviso s | Ativo s | Recuperação s | Leitura | Resposta |
|---|---|---|---|---|---|
| Garra do reservatório | 1.8 | 0.6 | 3.2 | braço trava e três marcas mostram área | desviar e puxar placa enquanto Zé descarrega |
| Anel de seca | 2.0 | 1.2 | 3.0 | anel abre faixa sem energia e sino grave | cruzar lacuna indicada por Trovico e regular válvula |

## Estados e fases

`Dormant → Intro → Telegraph → Active → Recover → Exposed → Reposition`. Ao perder contagem, `PhaseTransition` cancela hitboxes e projéteis antigos. Zero leva a `Defeated` uma única vez. Todo ataque tem deadline; não pode esperar para sempre por animação que não emitiu callback.

Fase 1: apenas primeiro ataque e dica visual de objetivo. Fase 2: após uma exposição, alternar as duas ações sem sobrepor seus intervalos ativos. Fase 3: após duas exposições, reposicionar elementos entre ciclos com 1,5 s de pausa; nenhum aviso fica menor que o valor da tabela. No chefe final: três exposições retiram blindagem e três estabilizam núcleo; resgate é seção separada com checkpoint.

## Regras específicas

Quatro canais são pré-condição da arena, com feedback antes de começar. Reserva de água não tem barra falsa que leva a softlock; falha recupera a fase atual e não destrói final canônico.

## Dificuldade e recuperação

Assistência aumenta aviso em 35%, reduz dano configurável a 0,5 unidade acumulada e permite ignorar cronômetro de fuga. Não remove o objetivo de aprender a resposta. Após três derrotas, oferecer dica e assistência sem punição. Voltar ao checkpoint restaura integridade do chefe da fase atual; recompensas de vitória são idempotentes. Pausa congela timers do confronto.

## QA

Testar bordas, salto mais curto, herói trocado, mascote fora de posição, callback de animação ausente, pausa no frame ativo, queda antes do golpe final e reentrada após vitória. Confirmar que cada hitbox coincide com área avisada em 30 e 60 FPS. Som do aviso continua legível com música desligada.
