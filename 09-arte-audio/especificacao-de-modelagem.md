# Guia de modelagem, rig, materiais e importação

## Detalhamento individual V4

O catálogo `11-contratos/modelagem.json` e `09-arte-audio/modelagem/INDICE.md` definem budgets, rig, sockets e aceite por identidade. Aprender o processo em `09-arte-audio/13-do-concept-ao-modelo-3d.md`, configurar rig/import em `09-arte-audio/14-rig-lod-e-importacao.md` e revisar entregas com `09-arte-audio/15-entrega-e-homologacao-3d.md`.

## Entrega por identidade

Pranchas PNG são direção visual. Para implementar, produzir arquivo fonte Blender e export FBX na baseline V4 e GLB opcional para revisão, malhas LOD, UVs, mapas e rig; prefabs e animações pertencem ao projeto Unity real. Arquivo fonte por identidade e metadados de origem/licença ficam no Git LFS. Não converter pixels em um falso GLB “pronto”.

## Escala e malha

Unidade metro, orientação e eixo configurados no export, pivô no solo central do corpo, transform aplicado, sem escala negativa. Conferir normals, tangents e smoothing, remover faces internas invisíveis, malha sem n-gons deformáveis. Rig humano: pelvis/spine/chest/neck/head, limbs, fingers conforme close-up; até 70 bones no herói como budget inicial, quatro influências por vértice. Chapéu rígido segue cabeça; poncho recebe poucos ossos auxiliares em vez de cloth caro por padrão. Laço usa spline e malha controladas pelo sistema, sem centenas de rigidbodies.

Faísca tem raiz, quadril, coluna, carapaça rígida, pernas, cauda, orelhas e face; não deformar placas como borracha. Trovico tem raiz, corpo, pescoço, cabeça, asas articuladas, cauda e pernas, rig separado de humanoide. Cascavel usa segmentos instanciados e spline; Oráculo e Máquina usam pivôs rígidos para anéis; não precisam de humanoid retarget. Guardião tem quatro apoios coerentes com a prancha.

## Materiais

URP Lit simplificado com base color sem sombra pintada excessiva, normal e máscara quando necessária. Até dois materiais no herói, três no chefe; material de olhos/efeito compartilhado. Microdetalhe da imagem não vira geometria obrigatória: bakear. Paleta define hierarquia, mas ameaça sempre combina pose e forma. Transparência de vidro e água deve minimizar overdraw; contornos emissivos com limite de brilho. Testar compressão e mipmaps em cada GPU alvo.

## Animação

30 amostras/s como baseline de clips, interpolação em runtime, root motion fora da plataforma; loops sem salto e transições com pé em apoio. Clip de ação tem preparo, contato e recuperação separados. Dano não nasce apenas do frame de evento de animação. Mãos precisam de sockets de ferramenta e IK opcional barato; não reposicionar corpo pela pose visual. Personagem troca sem penetrar geometria e preserva alvo do mecanismo.

## LOD e performance

Budgets das fichas são metas iniciais. Trocar LOD por tamanho na tela, reduzir rig/cabelo/props a distância e manter silhueta. Perfil baixo usa sombras bakeadas, partículas reduzidas, resolução dinâmica ou fixa menor após avaliação. Não prometer visual cinematográfico de concept no celular sem medir consumo, temperatura e memória.

## Checklist de aceite por prefab

Fonte + export versionados, escala/pivô corretos, LODs, UV/materiais sem faltas, rig e clips, sockets, collider separado da malha, máscaras de camada, endereço estável no catálogo, hash/licença, screenshot de teste e amostra no aparelho. A tarefa não conclui só por haver PNG no portal.
