# Zé Trovão — pacote de modelagem V4

ID `CHR-ZE-TROVAO` · Concept `09-assets/personagens/ze-trovao.png` · Estado: modelo/rig/animação não produzidos.

## Escala, composição e topologia

Altura de referência: 1.90 m (reference); excluir altura extra de chapéu/antenas se o blockout do ator assim definir e registrar a medida adotada. Raiz de gameplay no chão; visual filho com frente +Z no Unity e rotação de facing independente do collider no plano XY.

Tronco largo e botas pesadas; não escalar a malha de Mara para fabricar Zé. Poncho ferrugem, pena e bobina devem manter a silhueta. Prótese inexistente: não copiar o braço do Barão.

Fazer vistas técnicas ortográficas alinhadas, pose neutra e escala antes de esculpir. Se uma vista da concept contradizer outra, registrar e resolver em blockout; não propagar a inconsistência para rig. Validar silhueta em cinza e no enquadramento lateral real antes de texturizar. Retopologia deve concentrar loops onde há deformação; detalhes menores que a leitura em tela migram para bake.

## Rig, peças e sockets

Base humana compatível com Mara, proporções próprias; poncho e pena auxiliares. Avatar e clips retargetados precisam de correção de apoio dos pés.

Rig Unity: **Humanoid**. Teto inicial de 70 ossos deformadores e 4 influências por vértice. Controles do arquivo fonte não precisam ser exportados. Peças rígidas podem seguir transforms; seu custo não desaparece por não serem bones. Nomear sockets: `hand_r_tool`, `hand_l_grip`, `belt_charge`, `companion_anchor`. São filhos do visual/rig e não substituem pontos de lógica estáveis no root.

## Malha, UV e materiais

LOD0/1/2: **18000 / 9000 / 4500 triângulos** no conjunto, por nível, sem somar os três como geometria simultânea. Até 2 slots materiais no LOD0; mapa principal até 2048 px. Estes valores substituem os parágrafos genéricos de orçamento das versões anteriores para esta identidade.

Base color com iluminação limpa; normal e máscara de superfícies segundo o shader URP escolhido. UV sem sobreposição nas áreas de bake; espelhamento só onde aprovado. Acentos de tecido/cobre preservados e padrões de perigo legíveis também sem cor. Cada textura adicional tem custo de memória; dimensão 2K não significa automaticamente baixo custo.

## Animações a entregar

`idle_hat`, `walk_heavy`, `run`, `jump`, `land_heavy`, `impact`, `push_start`, `push_loop`, `push_end`, `ground_charge`, `hurt`, `hold_cable`.

Entrega in-place, baseline 30 amostras/s, com preparo, contato e recuperação legíveis. Loop sem salto no primeiro/último quadro; sem root drift acidental. O domínio controla dano/progresso e cancelamento; AnimationEvents só sincronizam apresentação. Tempos de confronto vêm da ficha do boss, sem redefini-los aqui.

## Aceite específico

Impacto tem antecipação e recuperação próprias; não deslocar o root para atravessar o chão. Push usa pose de contato ajustada à carga, com IK opcional.

## Arquivos e prova de entrega

Entregar fonte .blend com controles, export FBX com hierarquia/escala homologadas, texturas, clips e manifesto conforme `09-arte-audio/15-entrega-e-homologacao-3d.md`. GLB opcional serve a revisão; não é o caminho canônico de rig/animação Unity. Incluir captura neutra, silhueta lateral, teste de deformação e captura no aparelho do perfil alvo. O review deve mostrar o modelo em movimento, não só um render bonito.
