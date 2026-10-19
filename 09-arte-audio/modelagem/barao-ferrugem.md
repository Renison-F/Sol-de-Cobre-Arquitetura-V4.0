# Barão Ferrugem — pacote de modelagem V4

ID `CHR-BARAO-FERRUGEM` · Concept `09-assets/personagens/barao-ferrugem.png` · Estado: modelo/rig/animação não produzidos.

## Escala, composição e topologia

Altura de referência: 1.86 m (reference); excluir altura extra de chapéu/antenas se o blockout do ator assim definir e registrar a medida adotada. Raiz de gameplay no chão; visual filho com frente +Z no Unity e rotação de facing independente do collider no plano XY.

Casaco longo preto/bronze, bengala-medidor, reservatório âmbar e braço mecânico distintos. Não fechar o casaco em uma superfície que impeça passada ou resgate.

Fazer vistas técnicas ortográficas alinhadas, pose neutra e escala antes de esculpir. Se uma vista da concept contradizer outra, registrar e resolver em blockout; não propagar a inconsistência para rig. Validar silhueta em cinza e no enquadramento lateral real antes de texturizar. Retopologia deve concentrar loops onde há deformação; detalhes menores que a leitura em tela migram para bake.

## Rig, peças e sockets

Base humana com prótese rígida por pivôs e ossos de abas; mangueiras simplificadas; bengala com socket independente.

Rig Unity: **Humanoid**. Teto inicial de 70 ossos deformadores e 4 influências por vértice. Controles do arquivo fonte não precisam ser exportados. Peças rígidas podem seguir transforms; seu custo não desaparece por não serem bones. Nomear sockets: `hand_cane`, `command_origin`, `tank_anchor`, `rescue_grip`. São filhos do visual/rig e não substituem pontos de lógica estáveis no root.

## Malha, UV e materiais

LOD0/1/2: **18000 / 9000 / 4500 triângulos** no conjunto, por nível, sem somar os três como geometria simultânea. Até 2 slots materiais no LOD0; mapa principal até 2048 px. Estes valores substituem os parágrafos genéricos de orçamento das versões anteriores para esta identidade.

Base color com iluminação limpa; normal e máscara de superfícies segundo o shader URP escolhido. UV sem sobreposição nas áreas de bake; espelhamento só onde aprovado. Acentos de tecido/cobre preservados e padrões de perigo legíveis também sem cor. Cada textura adicional tem custo de memória; dimensão 2K não significa automaticamente baixo custo.

## Animações a entregar

`idle_cane`, `walk`, `balcony_talk`, `command`, `shield_react`, `stagger`, `fall_safe`, `rescue_grab`, `surrender_key`.

Entrega in-place, baseline 30 amostras/s, com preparo, contato e recuperação legíveis. Loop sem salto no primeiro/último quadro; sem root drift acidental. O domínio controla dano/progresso e cancelamento; AnimationEvents só sincronizam apresentação. Tempos de confronto vêm da ficha do boss, sem redefini-los aqui.

## Aceite específico

Command não é ataque corporal. Fall_safe e rescue_grab sem violência gráfica; o Barão não é destruído com a máquina.

## Arquivos e prova de entrega

Entregar fonte .blend com controles, export FBX com hierarquia/escala homologadas, texturas, clips e manifesto conforme `09-arte-audio/15-entrega-e-homologacao-3d.md`. GLB opcional serve a revisão; não é o caminho canônico de rig/animação Unity. Incluir captura neutra, silhueta lateral, teste de deformação e captura no aparelho do perfil alvo. O review deve mostrar o modelo em movimento, não só um render bonito.
