# Controles touch e remapeamento

Orientação landscape. Lado esquerdo: direcional virtual com raio inicial 72 dp e zona morta 0,15; alternativa de botões esquerda/direita. Lado direito: salto (64 dp), habilidade principal (56 dp), interação contextual (56 dp) e troca de herói (48 dp). Mascote usa a mesma interação quando o contexto está destacado; evitar quatro botões redundantes para cada mascote. Todos têm distância mínima de 12 dp, área interativa pelo menos 48 dp e respeito às safe areas reais do SO.

Dois dedos independentes: mover e saltar simultaneamente, habilidade não rouba pointer do salto. Perda de foco ou cancelamento de pointer zera eixos e botões presos. Arrastar dedo para fora do botão não deixa ação permanentemente ativa. Guardar canhoto, opacidade 50–100%, escala 80–140% e reposicionamento com botão restaurar.

Teclado: A/D ou setas, Espaço salta, E interage, Shift habilidade, Q troca, Esc pausa. Gamepad: stick/dpad move, sul salta, oeste habilidade, norte interage, ombro troca, start pausa. Mapear por ação no Input System; UI navega foco e mostra glifo do dispositivo usado. Não depender de código de tecla na regra de movimento.

Buffer de salto 150 ms e coyote 120 ms funcionam por relógio de simulação, não contagem de frames. Input coletado em Update fornece snapshot imutável ao motor; não movimentar rigidbody em múltiplos loops. Testar 30/60 FPS, toque na borda, três dedos, chamada do sistema e troca de orientação bloqueada.
