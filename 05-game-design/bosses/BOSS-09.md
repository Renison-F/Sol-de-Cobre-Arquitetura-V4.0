# BOSS-09 · Madame Engrenagem

Vinculado a LVL-09. Arte: `09-assets/personagens/madame-engrenagem.png`.

## Fantasia e vitória

Quebrar formação de drones sem colapsar o sistema e liberar comando de segurança. O confronto usa 3 exposições de núcleo como contagem de integridade; um acerto válido por exposição, sem DPS ilimitado ou exigência de grind. Dano ao herói: uma unidade; vida padrão 5. Sem colisão de contato enquanto chefe está inativo. O Barão é resgatado; máquinas são desativadas sem violência gráfica.

Arena: pátio industrial de 32 m, quatro consoles protegidos, duas passarelas altas. Cada posição ameaçada tem rota segura alcançável com os parâmetros do herói do capítulo.

## Ataques configuráveis

| Ação | Aviso s | Ativo s | Recuperação s | Leitura | Resposta |
|---|---|---|---|---|---|
| Formação tesoura | 1.8 | 1.0 | 2.8 | dois drones traçam linhas convergentes | permanecer no intervalo e prender nó de comando |
| Cercado de vapor | 1.5 | 1.2 | 3.0 | válvulas dos lados assobiam alternadas | subir passarela e desligar console marcado |

## Estados e fases

`Dormant → Intro → Telegraph → Active → Recover → Exposed → Reposition`. Ao perder contagem, `PhaseTransition` cancela hitboxes e projéteis antigos. Zero leva a `Defeated` uma única vez. Todo ataque tem deadline; não pode esperar para sempre por animação que não emitiu callback.

Fase 1: apenas primeiro ataque e dica visual de objetivo. Fase 2: após uma exposição, alternar as duas ações sem sobrepor seus intervalos ativos. Fase 3: após duas exposições, reposicionar elementos entre ciclos com 1,5 s de pausa; nenhum aviso fica menor que o valor da tabela. No chefe final: três exposições retiram blindagem e três estabilizam núcleo; resgate é seção separada com checkpoint.

## Regras específicas

Drones são parte do chefe, no máximo quatro ativos. Desligar comando interrompe formação inteira. Madame passa ao diálogo e à evacuação, não cai em animação de morte.

## Dificuldade e recuperação

Assistência aumenta aviso em 35%, reduz dano configurável a 0,5 unidade acumulada e permite ignorar cronômetro de fuga. Não remove o objetivo de aprender a resposta. Após três derrotas, oferecer dica e assistência sem punição. Voltar ao checkpoint restaura integridade do chefe da fase atual; recompensas de vitória são idempotentes. Pausa congela timers do confronto.

## QA

Testar bordas, salto mais curto, herói trocado, mascote fora de posição, callback de animação ausente, pausa no frame ativo, queda antes do golpe final e reentrada após vitória. Confirmar que cada hitbox coincide com área avisada em 30 e 60 FPS. Som do aviso continua legível com música desligada.
