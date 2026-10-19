# Luna Fagulha — pacote de modelagem V4

ID `CHR-LUNA-FAGULHA` · Concept `09-assets/personagens/luna-fagulha.png` · Estado: modelo/rig/animação não produzidos.

## Escala, composição e topologia

Altura de referência: 1.66 m (reference); excluir altura extra de chapéu/antenas se o blockout do ator assim definir e registrar a medida adotada. Raiz de gameplay no chão; visual filho com frente +Z no Unity e rotação de facing independente do collider no plano XY.

Tranças ruivas e óculos do chapéu em volumes simples; ferramentas reutilizam material de cobre. Condensador manual é instrumento de engenharia, mesmo quando a legenda da concept o chama de pistola.

Fazer vistas técnicas ortográficas alinhadas, pose neutra e escala antes de esculpir. Se uma vista da concept contradizer outra, registrar e resolver em blockout; não propagar a inconsistência para rig. Validar silhueta em cinza e no enquadramento lateral real antes de texturizar. Retopologia deve concentrar loops onde há deformação; detalhes menores que a leitura em tela migram para bake.

## Rig, peças e sockets

Base humana; tranças e poncho com poucos auxiliares; ferramentas rígidas com sockets, sem rig facial complexo obrigatório.

Rig Unity: **Humanoid**. Teto inicial de 60 ossos deformadores e 4 influências por vértice. Controles do arquivo fonte não precisam ser exportados. Peças rígidas podem seguir transforms; seu custo não desaparece por não serem bones. Nomear sockets: `hand_r_tool`, `hand_l_grip`, `belt_tool`, `repair_target`. São filhos do visual/rig e não substituem pontos de lógica estáveis no root.

## Malha, UV e materiais

LOD0/1/2: **12000 / 6000 / 3000 triângulos** no conjunto, por nível, sem somar os três como geometria simultânea. Até 2 slots materiais no LOD0; mapa principal até 1024 px. Estes valores substituem os parágrafos genéricos de orçamento das versões anteriores para esta identidade.

Base color com iluminação limpa; normal e máscara de superfícies segundo o shader URP escolhido. UV sem sobreposição nas áreas de bake; espelhamento só onde aprovado. Acentos de tecido/cobre preservados e padrões de perigo legíveis também sem cor. Cada textura adicional tem custo de memória; dimensão 2K não significa automaticamente baixo custo.

## Animações a entregar

`idle_tool`, `walk`, `examine`, `valve_turn`, `repair_loop`, `transmitter`, `warn`, `relieved`, `celebrate`.

Entrega in-place, baseline 30 amostras/s, com preparo, contato e recuperação legíveis. Loop sem salto no primeiro/último quadro; sem root drift acidental. O domínio controla dano/progresso e cancelamento; AnimationEvents só sincronizam apresentação. Tempos de confronto vêm da ficha do boss, sem redefini-los aqui.

## Aceite específico

Repair e valve_turn devem funcionar no socket do mecanismo. Não criar sistema de tiros nem tornar Luna jogável por causa do objeto da arte.

## Arquivos e prova de entrega

Entregar fonte .blend com controles, export FBX com hierarquia/escala homologadas, texturas, clips e manifesto conforme `09-arte-audio/15-entrega-e-homologacao-3d.md`. GLB opcional serve a revisão; não é o caminho canônico de rig/animação Unity. Incluir captura neutra, silhueta lateral, teste de deformação e captura no aparelho do perfil alvo. O review deve mostrar o modelo em movimento, não só um render bonito.
