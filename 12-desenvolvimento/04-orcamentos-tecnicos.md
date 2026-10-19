# Budgets e matriz de aparelhos

Números abaixo são metas iniciais a validar, não medições de um jogo inexistente. Perfil baixo: 30 FPS (33,3 ms), resolução de referência 720p, sombras principais bakeadas, uma luz principal, partículas reduzidas. Perfil médio: 60 FPS (16,7 ms), resolução de referência 900–1080p ajustada ao aparelho, sombras limitadas e efeitos seletivos. CPU/GPU podem trabalhar em paralelo; não somar tempos como custo linear.

| Item | Teto inicial |
|---|---|
| Memória residente de jogo | 1,2 GiB no perfil baixo, avaliar SO e pico de carga |
| Texturas residentes | 350 MiB comprimidas no perfil baixo |
| Triângulos visíveis | 250k baixo / 500k médio, medir custo de material e overdraw |
| Batches | 150 baixo / 250 médio, meta e não regra universal |
| Inimigos comuns | 6 ativos na janela |
| Figurantes animados | 8 no médio; reduzir no baixo |
| Drones de Madame | 4 como parte do boss |
| Fontes de áudio simultâneas | 24 baixo / 32 médio, com prioridades |
| Save por slot | 1 MiB máximo |
| Retomada de checkpoint | Alvo inferior a 5 s em dados já carregados |
| Resposta a comando | Alvo até 100 ms percebidos em dispositivo saudável |

Matriz mínima de QA por características: Android com 4 GB RAM/GPU Mali mais antiga; Android 6–8 GB/Adreno intermediária; Android recente 8+ GB e tela ultrawide; iPhone compatível mais antigo do escopo iOS16; iPhone intermediário e recente; tablet. A equipe deve registrar modelo, SO, GPU, build e temperatura reais ao selecionar aparelhos disponíveis. Não declarar que todos são suportados sem teste.

Sessão de 20–30 min verifica thermal throttling, bateria e memória; teste curto de FPS não basta. Usar captura de frame e profiler em cenas representativas de chefe, água e trem. Escolher Android minSDK 26 como meta inicial de produto (não requisito atual de submissão); targetSDK e Xcode/SDK exigidos pela loja são verificados novamente no release.
