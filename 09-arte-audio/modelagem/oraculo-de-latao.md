# Oráculo de Latão — pacote de modelagem V4

ID `BOSS-08` · Concept `09-assets/personagens/oraculo-de-latao.png` · Estado: modelo/rig/animação não produzidos.

## Escala, composição e topologia

Altura de referência: 2.60 m (proposed_blockout); excluir altura extra de chapéu/antenas se o blockout do ator assim definir e registrar a medida adotada. Raiz de gameplay no chão; visual filho com frente +Z no Unity e rotação de facing independente do collider no plano XY.

Corpo esférico/astrolábio, íris, anéis, pequenas esferas e tripé. Gravações estreladas em normal/base color; preservar vazios entre anéis.

Fazer vistas técnicas ortográficas alinhadas, pose neutra e escala antes de esculpir. Se uma vista da concept contradizer outra, registrar e resolver em blockout; não propagar a inconsistência para rig. Validar silhueta em cinza e no enquadramento lateral real antes de texturizar. Retopologia deve concentrar loops onde há deformação; detalhes menores que a leitura em tela migram para bake.

## Rig, peças e sockets

Root e três pernas; anéis com eixos locais explícitos; íris em lâminas/pivôs econômicos; evitar escala negativa no giro.

Rig Unity: **Generic**. Teto inicial de 28 ossos deformadores e 4 influências por vértice. Controles do arquivo fonte não precisam ser exportados. Peças rígidas podem seguir transforms; seu custo não desaparece por não serem bones. Nomear sockets: `lens_origin`, `ring_x`, `ring_y`, `ring_z`, `mirror_target`. São filhos do visual/rig e não substituem pontos de lógica estáveis no root.

## Malha, UV e materiais

LOD0/1/2: **18000 / 9000 / 4500 triângulos** no conjunto, por nível, sem somar os três como geometria simultânea. Até 3 slots materiais no LOD0; mapa principal até 2048 px. Estes valores substituem os parágrafos genéricos de orçamento das versões anteriores para esta identidade.

Base color com iluminação limpa; normal e máscara de superfícies segundo o shader URP escolhido. UV sem sobreposição nas áreas de bake; espelhamento só onde aprovado. Acentos de tecido/cobre preservados e padrões de perigo legíveis também sem cor. Cada textura adicional tem custo de memória; dimensão 2K não significa automaticamente baixo custo.

## Animações a entregar

`intro`, `telegraph_A`, `attack_A`, `recover_A`, `telegraph_B`, `attack_B`, `recover_B`, `exposed`, `defeated`, `idle`, `locomotion`.

Entrega in-place, baseline 30 amostras/s, com preparo, contato e recuperação legíveis. Loop sem salto no primeiro/último quadro; sem root drift acidental. O domínio controla dano/progresso e cancelamento; AnimationEvents só sincronizam apresentação. Tempos de confronto vêm da ficha do boss, sem redefini-los aqui.

## Aceite específico

Anéis giram sem varrer plataformas seguras. Feixes e espelhos são lógica de puzzle, não raycasts emitidos por cada frame de animação.

## Arquivos e prova de entrega

Entregar fonte .blend com controles, export FBX com hierarquia/escala homologadas, texturas, clips e manifesto conforme `09-arte-audio/15-entrega-e-homologacao-3d.md`. GLB opcional serve a revisão; não é o caminho canônico de rig/animação Unity. Incluir captura neutra, silhueta lateral, teste de deformação e captura no aparelho do perfil alvo. O review deve mostrar o modelo em movimento, não só um render bonito.
