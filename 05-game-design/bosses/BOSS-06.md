# BOSS-06 · Guardião dos Canais

Vinculado a LVL-06. Arte: `09-assets/personagens/guardiao-dos-canais.png`.

## Fantasia e vitória

Equalizar três reservatórios, corrigir leitura do guardião e preservar comportas. O confronto usa 3 exposições de núcleo como contagem de integridade; um acerto válido por exposição, sem DPS ilimitado ou exigência de grind. Dano ao herói: uma unidade; vida padrão 5. Sem colisão de contato enquanto chefe está inativo. O Barão é resgatado; máquinas são desativadas sem violência gráfica.

Arena: bacia de 30 m com três canais em alturas 0/1,5/3 m e ilhas seguras. Cada posição ameaçada tem rota segura alcançável com os parâmetros do herói do capítulo.

## Ataques configuráveis

| Ação | Aviso s | Ativo s | Recuperação s | Leitura | Resposta |
|---|---|---|---|---|---|
| Jato de limpeza | 1.6 | 0.8 | 2.6 | reservatório enche e bocal aponta | subir ilha com seta e girar válvula exposta |
| Pulso de pressão | 1.5 | 0.5 | 2.8 | três medidores sobem com clique | baixar válvula do tanque correspondente; sem dano se pressão correta |

## Estados e fases

`Dormant → Intro → Telegraph → Active → Recover → Exposed → Reposition`. Ao perder contagem, `PhaseTransition` cancela hitboxes e projéteis antigos. Zero leva a `Defeated` uma única vez. Todo ataque tem deadline; não pode esperar para sempre por animação que não emitiu callback.

Fase 1: apenas primeiro ataque e dica visual de objetivo. Fase 2: após uma exposição, alternar as duas ações sem sobrepor seus intervalos ativos. Fase 3: após duas exposições, reposicionar elementos entre ciclos com 1,5 s de pausa; nenhum aviso fica menor que o valor da tabela. No chefe final: três exposições retiram blindagem e três estabilizam núcleo; resgate é seção separada com checkpoint.

## Regras específicas

Este chefe se recalibra, não morre. Resolver pressão tem mesma validade que reduzir integridade por exposição. Não descarregar água dos canais já reparados ao reiniciar.

## Dificuldade e recuperação

Assistência aumenta aviso em 35%, reduz dano configurável a 0,5 unidade acumulada e permite ignorar cronômetro de fuga. Não remove o objetivo de aprender a resposta. Após três derrotas, oferecer dica e assistência sem punição. Voltar ao checkpoint restaura integridade do chefe da fase atual; recompensas de vitória são idempotentes. Pausa congela timers do confronto.

## QA

Testar bordas, salto mais curto, herói trocado, mascote fora de posição, callback de animação ausente, pausa no frame ativo, queda antes do golpe final e reentrada após vitória. Confirmar que cada hitbox coincide com área avisada em 30 e 60 FPS. Som do aviso continua legível com música desligada.
