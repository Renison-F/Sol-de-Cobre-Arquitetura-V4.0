# General Para-Raios — pacote de modelagem V4

ID `BOSS-07` · Concept `09-assets/personagens/general-para-raios.png` · Estado: modelo/rig/animação não produzidos.

## Escala, composição e topologia

Altura de referência: 3.40 m (proposed_blockout); excluir altura extra de chapéu/antenas se o blockout do ator assim definir e registrar a medida adotada. Raiz de gameplay no chão; visual filho com frente +Z no Unity e rotação de facing independente do collider no plano XY.

Casaco militar, ombreiras, três bobinas dorsais e manoplas. Energia é VFX separado; bobinas não exigem fios helicoidais de alto polígono a distância.

Fazer vistas técnicas ortográficas alinhadas, pose neutra e escala antes de esculpir. Se uma vista da concept contradizer outra, registrar e resolver em blockout; não propagar a inconsistência para rig. Validar silhueta em cinza e no enquadramento lateral real antes de texturizar. Retopologia deve concentrar loops onde há deformação; detalhes menores que a leitura em tela migram para bake.

## Rig, peças e sockets

Base mecânica bípede; ombros e casaco auxiliares; bobinas com pivôs e sockets de descarga.

Rig Unity: **Generic**. Teto inicial de 52 ossos deformadores e 4 influências por vértice. Controles do arquivo fonte não precisam ser exportados. Peças rígidas podem seguir transforms; seu custo não desaparece por não serem bones. Nomear sockets: `coil_l`, `coil_c`, `coil_r`, `discharge_origin`. São filhos do visual/rig e não substituem pontos de lógica estáveis no root.

## Malha, UV e materiais

LOD0/1/2: **25000 / 12500 / 6250 triângulos** no conjunto, por nível, sem somar os três como geometria simultânea. Até 3 slots materiais no LOD0; mapa principal até 2048 px. Estes valores substituem os parágrafos genéricos de orçamento das versões anteriores para esta identidade.

Base color com iluminação limpa; normal e máscara de superfícies segundo o shader URP escolhido. UV sem sobreposição nas áreas de bake; espelhamento só onde aprovado. Acentos de tecido/cobre preservados e padrões de perigo legíveis também sem cor. Cada textura adicional tem custo de memória; dimensão 2K não significa automaticamente baixo custo.

## Animações a entregar

`intro`, `telegraph_A`, `attack_A`, `recover_A`, `telegraph_B`, `attack_B`, `recover_B`, `exposed`, `defeated`, `idle`, `locomotion`.

Entrega in-place, baseline 30 amostras/s, com preparo, contato e recuperação legíveis. Loop sem salto no primeiro/último quadro; sem root drift acidental. O domínio controla dano/progresso e cancelamento; AnimationEvents só sincronizam apresentação. Tempos de confronto vêm da ficha do boss, sem redefini-los aqui.

## Aceite específico

Ataques seguem marcação, áudio e contagem de BOSS-07. Testar reduced_flashes; não bakear clarões obrigatórios na textura.

## Arquivos e prova de entrega

Entregar fonte .blend com controles, export FBX com hierarquia/escala homologadas, texturas, clips e manifesto conforme `09-arte-audio/15-entrega-e-homologacao-3d.md`. GLB opcional serve a revisão; não é o caminho canônico de rig/animação Unity. Incluir captura neutra, silhueta lateral, teste de deformação e captura no aparelho do perfil alvo. O review deve mostrar o modelo em movimento, não só um render bonito.
