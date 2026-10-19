# Faísca — ficha de produção V3

ID `CHR-FAISCA` · Companheiro contextual de Mara. Baseline autoral para implementação, sem afirmação de aprovação pelo usuário.

## Identidade visual

Tatu terracota com carapaça segmentada, orelhas grandes, lenço turquesa e mochila. Altura de referência 0.62 m; unidade Unity = metro. Para Trovico medir altura pousado e estabelecer envergadura no blockout; para Faísca medir ereto na pose de referência. Escala do collider de gameplay é calibrada separadamente da aba do chapéu e penas.

Arte-mestra: `09-assets/personagens/faisca-tatu.png`. Manter cor, forma de rosto e acessórios centrais nas variações; desenho ortográfico técnico final deve resolver inconsistências de perspectiva antes da modelagem. Concept art renderizada não é prova de malha, UV, rig ou animação final.

## Motivação e arco

Objetivo: Proteger a dupla e descobrir passagens. Entra em túneis por impulso; aprende a avisar antes de sumir. Relações e consequências seguem `01-universo/08-canon-v3.md`. Não criar cenas que contradigam a sobrevivência de Isadora ou a resolução comunitária.

## Função no jogo

Escava canais marcados e encontra objetos; não transporta herói. Executa ações de suporte por comando contextual ou cena; não exige mais um conjunto permanente de botões. Ação contextual precisa de alcance válido, alvo visível e resultado claramente indicado. Erro de comando não consome recurso nem causa softlock.

## Voz e atuação

Sem fala humana; assobios, fungadas e gestos legíveis. Fala de referência: [cheira a terra, aponta e espera Mara]. Expressão reage à situação antes da explicação. Durante travessia, no máximo duas linhas curtas na tela e legenda não cobre próximo apoio. Mascotes comunicam-se por som e gesto.

## Lista de animações

idle_sniff, walk, run, curl, roll, dig_enter, dig_loop, dig_exit, point, celebrate, puddle_startle. Cada clipe tem pose inicial/final de blend definida, eventos apenas para VFX/áudio e regra de domínio como autoridade de dano/progresso. Root motion desligado para heróis de plataforma; trajetória pertence ao motor. Testar pé deslizando, chapéu cruzando cabeça, poncho atravessando cinto e mão desalinhada no laço/ferramenta.

## Entrega técnica por asset

Budget específico V4: 8000/4000/2000 triângulos em LOD0/1/2, até 2 slots materiais e 42 ossos deformadores. Metas iniciais a validar; fonte única e exceções em `09-arte-audio/modelagem/faisca.md`.

## Aceite

Silhueta legível a 720p, pose neutra consistente, acessórios não ocultam hitbox, LOD sem salto gritante, animação funciona a 30/60 FPS, materiais no URP homologado, nenhum Missing Script. Revisar licença e origem de recursos usados. Produção 3D e homologação são tarefas de desenvolvimento explicitamente rastreadas no backlog.


## Pacote técnico de modelagem V4

A especificação individual de geometria, rig, sockets, animações e aceite está em `09-arte-audio/modelagem/faisca.md`. Seus budgets específicos substituem os valores genéricos acima. Fonte numérica única: `11-contratos/modelagem.json`.
