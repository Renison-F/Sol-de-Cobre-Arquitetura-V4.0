# Barão Ferrugem — ficha de produção V3

ID `CHR-BARAO-FERRUGEM` · Antagonista, BOSS-10. Baseline autoral para implementação, sem afirmação de aprovação pelo usuário.

## Identidade visual

Casaco preto longo, detalhes bronze, cabelo prata, bengala-medidor e reservatório âmbar. Altura de referência 1.86 m; unidade Unity = metro. Para Trovico medir altura pousado e estabelecer envergadura no blockout; para Faísca medir ereto na pose de referência. Escala do collider de gameplay é calibrada separadamente da aba do chapéu e penas.

Arte-mestra: `09-assets/personagens/barao-ferrugem.png`. Manter cor, forma de rosto e acessórios centrais nas variações; desenho ortográfico técnico final deve resolver inconsistências de perspectiva antes da modelagem. Concept art renderizada não é prova de malha, UV, rig ou animação final.

## Motivação e arco

Objetivo: Controlar a chuva para evitar a escassez que presenciou. Confunde proteção com domínio; perde monopólio e responde pela reparação. Relações e consequências seguem `01-universo/08-canon-v3.md`. Não criar cenas que contradigam a sobrevivência de Isadora ou a resolução comunitária.

## Função no jogo

Comanda Máquina da Seca; não é alvo de violência gráfica. Executa ações de suporte por comando contextual ou cena; não exige mais um conjunto permanente de botões. Ação contextual precisa de alcance válido, alvo visível e resultado claramente indicado. Erro de comando não consome recurso nem causa softlock.

## Voz e atuação

Cortês e controlador, usa estatísticas para evitar falar de pessoas. Fala de referência: “O céu não negocia. Por isso eu precisei dar ordens.”. Expressão reage à situação antes da explicação. Durante travessia, no máximo duas linhas curtas na tela e legenda não cobre próximo apoio. Mascotes comunicam-se por som e gesto.

## Lista de animações

idle_cane, walk, balcony_talk, command, shield_react, stagger, fall_safe, rescue_grab, surrender_key. Cada clipe tem pose inicial/final de blend definida, eventos apenas para VFX/áudio e regra de domínio como autoridade de dano/progresso. Root motion desligado para heróis de plataforma; trajetória pertence ao motor. Testar pé deslizando, chapéu cruzando cabeça, poncho atravessando cinto e mão desalinhada no laço/ferramenta.

## Entrega técnica por asset

Budget específico V4: 18000/9000/4500 triângulos em LOD0/1/2, até 2 slots materiais e 70 ossos deformadores. Metas iniciais a validar; fonte única e exceções em `09-arte-audio/modelagem/barao-ferrugem.md`.

## Aceite

Silhueta legível a 720p, pose neutra consistente, acessórios não ocultam hitbox, LOD sem salto gritante, animação funciona a 30/60 FPS, materiais no URP homologado, nenhum Missing Script. Revisar licença e origem de recursos usados. Produção 3D e homologação são tarefas de desenvolvimento explicitamente rastreadas no backlog.


## Pacote técnico de modelagem V4

A especificação individual de geometria, rig, sockets, animações e aceite está em `09-arte-audio/modelagem/barao-ferrugem.md`. Seus budgets específicos substituem os valores genéricos acima. Fonte numérica única: `11-contratos/modelagem.json`.
