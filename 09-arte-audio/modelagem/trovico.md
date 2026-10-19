# Trovico — pacote de modelagem V4

ID `CHR-TROVICO` · Concept `09-assets/personagens/trovico-falcao.png` · Estado: modelo/rig/animação não produzidos.

## Escala, composição e topologia

Altura de referência: 0.75 m (reference); excluir altura extra de chapéu/antenas se o blockout do ator assim definir e registrar a medida adotada. Raiz de gameplay no chão; visual filho com frente +Z no Unity e rotação de facing independente do collider no plano XY.

Falcão estilizado; penas em grupos de volumes opacos e poucas lâminas. Abrir asas no turnaround técnico: a prancha pousada não define a envergadura.

Fazer vistas técnicas ortográficas alinhadas, pose neutra e escala antes de esculpir. Se uma vista da concept contradizer outra, registrar e resolver em blockout; não propagar a inconsistência para rig. Validar silhueta em cinza e no enquadramento lateral real antes de texturizar. Retopologia deve concentrar loops onde há deformação; detalhes menores que a leitura em tela migram para bake.

## Rig, peças e sockets

Root, corpo, pescoço, cabeça, ombro/cotovelo/pulso das asas, cauda e pernas. Envergadura inicial proposta: 1,4 m, a validar com a câmera.

Rig Unity: **Generic**. Teto inicial de 46 ossos deformadores e 4 influências por vértice. Controles do arquivo fonte não precisam ser exportados. Peças rígidas podem seguir transforms; seu custo não desaparece por não serem bones. Nomear sockets: `beak_target`, `perch_feet`, `wing_tip_l`, `wing_tip_r`. São filhos do visual/rig e não substituem pontos de lógica estáveis no root.

## Malha, UV e materiais

LOD0/1/2: **8000 / 4000 / 2000 triângulos** no conjunto, por nível, sem somar os três como geometria simultânea. Até 2 slots materiais no LOD0; mapa principal até 1024 px. Estes valores substituem os parágrafos genéricos de orçamento das versões anteriores para esta identidade.

Base color com iluminação limpa; normal e máscara de superfícies segundo o shader URP escolhido. UV sem sobreposição nas áreas de bake; espelhamento só onde aprovado. Acentos de tecido/cobre preservados e padrões de perigo legíveis também sem cor. Cada textura adicional tem custo de memória; dimensão 2K não significa automaticamente baixo custo.

## Animações a entregar

`perch_idle`, `takeoff`, `flap_loop`, `glide`, `hover`, `target_mark`, `switch_tap`, `return`, `land`, `celebrate`.

Entrega in-place, baseline 30 amostras/s, com preparo, contato e recuperação legíveis. Loop sem salto no primeiro/último quadro; sem root drift acidental. O domínio controla dano/progresso e cancelamento; AnimationEvents só sincronizam apresentação. Tempos de confronto vêm da ficha do boss, sem redefini-los aqui.

## Aceite específico

Pouso e decolagem precisam alinhar pés/poleiro. Voo segue trajetória do gameplay; bater asas não concede alcance extra nem transporta o herói.

## Arquivos e prova de entrega

Entregar fonte .blend com controles, export FBX com hierarquia/escala homologadas, texturas, clips e manifesto conforme `09-arte-audio/15-entrega-e-homologacao-3d.md`. GLB opcional serve a revisão; não é o caminho canônico de rig/animação Unity. Incluir captura neutra, silhueta lateral, teste de deformação e captura no aparelho do perfil alvo. O review deve mostrar o modelo em movimento, não só um render bonito.
