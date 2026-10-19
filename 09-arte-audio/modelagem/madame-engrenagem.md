# Madame Engrenagem — pacote de modelagem V4

ID `BOSS-09` · Concept `09-assets/personagens/madame-engrenagem.png` · Estado: modelo/rig/animação não produzidos.

## Escala, composição e topologia

Altura de referência: 1.76 m (proposed_blockout); excluir altura extra de chapéu/antenas se o blockout do ator assim definir e registrar a medida adotada. Raiz de gameplay no chão; visual filho com frente +Z no Unity e rotação de facing independente do collider no plano XY.

Mulher humana madura de casaco vinho, broche, bastão e drones separados. Não transformá-la em robô nem usar o mesmo rosto de Isadora. Drone compartilhado: teto adicional 1,5k tri e 12 pivôs por instância, quatro ativos.

Fazer vistas técnicas ortográficas alinhadas, pose neutra e escala antes de esculpir. Se uma vista da concept contradizer outra, registrar e resolver em blockout; não propagar a inconsistência para rig. Validar silhueta em cinza e no enquadramento lateral real antes de texturizar. Retopologia deve concentrar loops onde há deformação; detalhes menores que a leitura em tela migram para bake.

## Rig, peças e sockets

Base humana; casaco com auxiliares; bastão rígido. Drone é prefab independente com hélices e collider simplificado, sob orçamento próprio.

Rig Unity: **Humanoid**. Teto inicial de 70 ossos deformadores e 4 influências por vértice. Controles do arquivo fonte não precisam ser exportados. Peças rígidas podem seguir transforms; seu custo não desaparece por não serem bones. Nomear sockets: `hand_baton`, `drone_anchor_1`, `drone_anchor_2`, `command_origin`. São filhos do visual/rig e não substituem pontos de lógica estáveis no root.

## Malha, UV e materiais

LOD0/1/2: **18000 / 9000 / 4500 triângulos** no conjunto, por nível, sem somar os três como geometria simultânea. Até 3 slots materiais no LOD0; mapa principal até 2048 px. Estes valores substituem os parágrafos genéricos de orçamento das versões anteriores para esta identidade.

Base color com iluminação limpa; normal e máscara de superfícies segundo o shader URP escolhido. UV sem sobreposição nas áreas de bake; espelhamento só onde aprovado. Acentos de tecido/cobre preservados e padrões de perigo legíveis também sem cor. Cada textura adicional tem custo de memória; dimensão 2K não significa automaticamente baixo custo.

## Animações a entregar

`intro`, `telegraph_A`, `attack_A`, `recover_A`, `telegraph_B`, `attack_B`, `recover_B`, `exposed`, `defeated`, `idle`, `locomotion`.

Entrega in-place, baseline 30 amostras/s, com preparo, contato e recuperação legíveis. Loop sem salto no primeiro/último quadro; sem root drift acidental. O domínio controla dano/progresso e cancelamento; AnimationEvents só sincronizam apresentação. Tempos de confronto vêm da ficha do boss, sem redefini-los aqui.

## Aceite específico

Ataques são comandos para drones; encerramento por decisão e evacuação. Substituir leitura genérica de defeated por surrender/evacuate na camada de apresentação, sem animação de morte.

## Arquivos e prova de entrega

Entregar fonte .blend com controles, export FBX com hierarquia/escala homologadas, texturas, clips e manifesto conforme `09-arte-audio/15-entrega-e-homologacao-3d.md`. GLB opcional serve a revisão; não é o caminho canônico de rig/animação Unity. Incluir captura neutra, silhueta lateral, teste de deformação e captura no aparelho do perfil alvo. O review deve mostrar o modelo em movimento, não só um render bonito.
