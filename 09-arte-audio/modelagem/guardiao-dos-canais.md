# Guardião dos Canais — pacote de modelagem V4

ID `BOSS-06` · Concept `09-assets/personagens/guardiao-dos-canais.png` · Estado: modelo/rig/animação não produzidos.

## Escala, composição e topologia

Altura de referência: 3.20 m (proposed_blockout); excluir altura extra de chapéu/antenas se o blockout do ator assim definir e registrar a medida adotada. Raiz de gameplay no chão; visual filho com frente +Z no Unity e rotação de facing independente do collider no plano XY.

Corpo quadrúpede pétreo, válvulas de cobre e reservatório de água. Separar pedra opaca e janela de água; usar interior simplificado e minimizar transparências sobrepostas.

Fazer vistas técnicas ortográficas alinhadas, pose neutra e escala antes de esculpir. Se uma vista da concept contradizer outra, registrar e resolver em blockout; não propagar a inconsistência para rig. Validar silhueta em cinza e no enquadramento lateral real antes de texturizar. Retopologia deve concentrar loops onde há deformação; detalhes menores que a leitura em tela migram para bake.

## Rig, peças e sockets

Root, corpo, quatro apoios, pescoço/cabeça e válvulas rígidas. Uma única base locomotora mantém os quatro contatos coerentes.

Rig Unity: **Generic**. Teto inicial de 52 ossos deformadores e 4 influências por vértice. Controles do arquivo fonte não precisam ser exportados. Peças rígidas podem seguir transforms; seu custo não desaparece por não serem bones. Nomear sockets: `valve_l`, `valve_r`, `water_level`, `pressure_core`. São filhos do visual/rig e não substituem pontos de lógica estáveis no root.

## Malha, UV e materiais

LOD0/1/2: **25000 / 12500 / 6250 triângulos** no conjunto, por nível, sem somar os três como geometria simultânea. Até 3 slots materiais no LOD0; mapa principal até 2048 px. Estes valores substituem os parágrafos genéricos de orçamento das versões anteriores para esta identidade.

Base color com iluminação limpa; normal e máscara de superfícies segundo o shader URP escolhido. UV sem sobreposição nas áreas de bake; espelhamento só onde aprovado. Acentos de tecido/cobre preservados e padrões de perigo legíveis também sem cor. Cada textura adicional tem custo de memória; dimensão 2K não significa automaticamente baixo custo.

## Animações a entregar

`intro`, `telegraph_A`, `attack_A`, `recover_A`, `telegraph_B`, `attack_B`, `recover_B`, `exposed`, `defeated`, `idle`, `locomotion`.

Entrega in-place, baseline 30 amostras/s, com preparo, contato e recuperação legíveis. Loop sem salto no primeiro/último quadro; sem root drift acidental. O domínio controla dano/progresso e cancelamento; AnimationEvents só sincronizam apresentação. Tempos de confronto vêm da ficha do boss, sem redefini-los aqui.

## Aceite específico

Nível de água é parâmetro visual ligado ao estado da pressão; não é líquido simulado. Equalização pacifica o guardião; preservar a saída da arena.

## Arquivos e prova de entrega

Entregar fonte .blend com controles, export FBX com hierarquia/escala homologadas, texturas, clips e manifesto conforme `09-arte-audio/15-entrega-e-homologacao-3d.md`. GLB opcional serve a revisão; não é o caminho canônico de rig/animação Unity. Incluir captura neutra, silhueta lateral, teste de deformação e captura no aparelho do perfil alvo. O review deve mostrar o modelo em movimento, não só um render bonito.
