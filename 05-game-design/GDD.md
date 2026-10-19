# Game Design Document — Sol de Cobre V3

## Produto e promessa

Aventura premium individual 2.5D de Velho Oeste fantástico para Android/iOS, em orientação horizontal. Campanha de dez capítulos, cerca de 5 h 40 min de percurso estimado sem exploração extensa; meta 6–8 h com segredos e repetição. Tempos são planejamento, não duração medida. A referência de plataforma serve ao ritmo; personagens, história, cenários e recursos visuais são autorais.

Mara e Zé são controláveis; Faísca e Trovico são comandos contextuais; Luna e Isadora são personagens de história e suporte, não heróis jogáveis no lançamento. Troca de herói é individual e substitui avatar em ponto seguro, sem multiplayer local ou online. Dois conjuntos de habilidades não significam dois jogadores simultâneos.

## Pilares

Movimento responsivo com espaço para erro; narrativa contada enquanto se explora; cooperação por habilidades complementares; máquinas divertidas com consequências humanas; água compartilhada como resolução do conflito. Sem violência gráfica, armas de fogo controladas pelo jogador, loot aleatório comprável ou lives limitadas.

## Loop

Explorar → ler obstáculo → usar movimento/mascote → coletar pista → ativar checkpoint → resolver encontro → registrar reparação → abrir próximo capítulo. Desafio ensina uma habilidade em segurança antes de combiná-la. Nenhuma rota obrigatória exige recurso consumível ou precisão que só funciona a 60 FPS.

## Parâmetros iniciais

1 unidade Unity = 1 m. Plano XY, profundidade Z presa ao corredor. Mara: 6 m/s, aceleração 30 m/s², desaceleração 40 m/s², salto 2,6 m, gravidade −25 m/s². Zé: 5,2 m/s, aceleração 26, salto 2,4 m, mesma gravidade. Coyote time 0,12 s e buffer de salto 0,15 s; reduzir velocidade vertical ao soltar salto com fator 0,5. Esses números precisam de feel test.

Vida 5 unidades, dano comum 1, invulnerabilidade 1,2 s. Cair retorna ao checkpoint; sem game over que apaga progresso. Colecionáveis únicos permanecem após morte. Hit de contato de chefe só existe em estados ativos avisados. Assistência mantém as regras de objetivo e facilita janelas/dano.

## Habilidades

Laço: alvo visível até 7 m em cone de 50°, seleção por alinhamento/distância; interagir somente em argolas marcadas. Impulso de Zé: raio 2 m em solo, cooldown 1 s; quebra apenas rocha marcada. Aterramento exige placa apropriada e input segurar até carga estabilizar, no máximo 2 s. Faísca: túnel marcado até 6 m, ação 1,5 s, retorno 2 s; sem consumo de energia. Trovico: alvo em linha de visão até 8 m, ida/volta até 3 s, cooldown 3 s. Luna opera dispositivos em cenas de suporte sem botão permanente novo.

## Progressão e economia

Habilidades são desbloqueadas pela história, nunca por grind. Dez insígnias de capítulo e três fragmentos de Atlas por capítulo (30 no total); oito quests opcionais dão páginas de contexto e variação do epílogo. Fragmentos não compram poder. Não há loja interna na V3; roupas desbloqueáveis podem existir depois como decisão de escopo, sem economia já presumida.

## Câmera, interface e som

Câmera lateral com look-ahead de 2 m, amortecimento 0,18 s, dead zone de 1,2 m, limites por volume e enquadramento de arena. Sem tremor obrigatório. HUD contém vida, próximo objetivo e botões contextuais legíveis; menus suportam controle e touch. Legendas ajustáveis, símbolos além de cor, mix separado e redução de flashes.

## Escopo fechado do lançamento

Dez capítulos, dez chefes, sete personagens centrais, 19 variantes de inimigos comuns em quatro famílias de comportamento, três famílias visuais de figurantes sem nome, oito opcionais, seleção de capítulo, três slots de save, PT-BR e estrutura de localização. Não inclui multiplayer, mundo aberto, ranking, anúncios, nuvem, DLC ou idioma dublado final. Excluir esses itens é decisão explícita de escopo, não omissão escondida.

## Primeiro slice

Uma rota de 8–12 min da Estação, Mara/Faísca, dois inimigos, Fiscal, menu, controles touch, checkpoint e fluxo de licença sandbox. Se movimento, legibilidade ou experiência de compra não passarem no aparelho alvo, não produzir nove capítulos antes de corrigir o núcleo.
