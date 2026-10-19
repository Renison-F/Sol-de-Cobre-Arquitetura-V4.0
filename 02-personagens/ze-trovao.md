# Zé Trovão — ficha de produção V3

ID `CHR-ZE-TROVAO` · Coprotagonista jogável. Baseline autoral para implementação, sem afirmação de aprovação pelo usuário.

## Identidade visual

Silhueta larga, poncho ferrugem, chapéu com pena azul, bigode e barba curta, bobina no cinto. Altura de referência 1.90 m; unidade Unity = metro. Para Trovico medir altura pousado e estabelecer envergadura no blockout; para Faísca medir ereto na pose de referência. Escala do collider de gameplay é calibrada separadamente da aba do chapéu e penas.

Arte-mestra: `09-assets/personagens/ze-trovao.png`. Manter cor, forma de rosto e acessórios centrais nas variações; desenho ortográfico técnico final deve resolver inconsistências de perspectiva antes da modelagem. Concept art renderizada não é prova de malha, UV, rig ou animação final.

## Motivação e arco

Objetivo: Reparar a consequência do desvio que autorizou. Esconde culpa em bravatas; assume responsabilidade em ação. Relações e consequências seguem `01-universo/08-canon-v3.md`. Não criar cenas que contradigam a sobrevivência de Isadora ou a resolução comunitária.

## Função no jogo

Impacto no solo, empurrar carga e conduzir energia em placas. Entrada controlada pelo jogador; parâmetros no GDD e controles touch. Ação contextual precisa de alcance válido, alvo visível e resultado claramente indicado. Erro de comando não consome recurso nem causa softlock.

## Voz e atuação

Fanfarrão no início, hesita ao falar da ferrovia, direto após confissão. Fala de referência: “Desta vez eu fico até terminar.”. Expressão reage à situação antes da explicação. Durante travessia, no máximo duas linhas curtas na tela e legenda não cobre próximo apoio. Mascotes comunicam-se por som e gesto.

## Lista de animações

idle_hat, walk_heavy, run, jump, land_heavy, impact, push_start, push_loop, push_end, ground_charge, hurt, hold_cable. Cada clipe tem pose inicial/final de blend definida, eventos apenas para VFX/áudio e regra de domínio como autoridade de dano/progresso. Root motion desligado para heróis de plataforma; trajetória pertence ao motor. Testar pé deslizando, chapéu cruzando cabeça, poncho atravessando cinto e mão desalinhada no laço/ferramenta.

## Entrega técnica por asset

Budget específico V4: 18000/9000/4500 triângulos em LOD0/1/2, até 2 slots materiais e 70 ossos deformadores. Metas iniciais a validar; fonte única e exceções em `09-arte-audio/modelagem/ze-trovao.md`.

## Aceite

Silhueta legível a 720p, pose neutra consistente, acessórios não ocultam hitbox, LOD sem salto gritante, animação funciona a 30/60 FPS, materiais no URP homologado, nenhum Missing Script. Revisar licença e origem de recursos usados. Produção 3D e homologação são tarefas de desenvolvimento explicitamente rastreadas no backlog.


## Pacote técnico de modelagem V4

A especificação individual de geometria, rig, sockets, animações e aceite está em `09-arte-audio/modelagem/ze-trovao.md`. Seus budgets específicos substituem os valores genéricos acima. Fonte numérica única: `11-contratos/modelagem.json`.
