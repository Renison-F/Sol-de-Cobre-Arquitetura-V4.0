# Rig, animação e importação Blender → Unity

## Convenção do projeto

Unity usa metro; root do ator no chão, escala (1,1,1), movimento no plano XY com Z estável. O visual é filho do root e tem frente local +Z após importação homologada. Virar para direita/esquerda gira o visual; não usar escala negativa no root/collider. No Blender, trabalhar em metros, registrar orientação e preset FBX. Fazer round-trip com cubo de 1 m, frente marcada, um osso e um clip antes de exportar o elenco. A validação do preset prevalece sobre uma receita de eixos copiada sem teste.

## Humanos e Generic

Humanoid é baseline de Mara, Zé, Luna, Isadora, Barão e Madame. Criar Avatar por personagem e conferir mapeamento, postura e apoio; retarget não garante contato exato com chão/ferramenta. As proporções de Zé exigem correção de passada. Generic mantém hierarquias específicas para Faísca, Trovico e máquinas. Ossos/pivôs auxiliares não podem sumir na otimização do importador se scripts ou sockets dependem deles.

Não animar a origem do ator por engano: root motion desligado na plataforma; clips in-place. Um abraço pode usar um sistema separado de alinhamento/cinemática com entrada e saída controladas, sem transformar a física inteira em root motion. Dano, janela de vulnerabilidade, vitória e save pertencem à lógica, não a AnimationEvents. Esses eventos podem tocar som/poeira no quadro do contato.

## Especificação de clip

Cada clip registra ID, primeiro/último quadro, taxa, duração, loop, root motion, socket de contato e quais estados permitem cancelá-lo. Baseline de amostragem: 30 Hz; a execução não fica limitada a 30 FPS. Corrida e idle fazem loop; jump_start/land/ataques têm final definido. Testar pausa, corte de cinematografia, retorno do app e descarregamento durante ataque para limpar hitboxes/áudio.

## Materiais e texturas

FBX é export canônico desta baseline para malha/rig/clips. .blend é fonte editável fora de Assets para evitar reimportações dependentes da instalação local do Blender. GLB é preview opcional; só entra no jogo com importador escolhido, licença e comportamento homologados em ADR. Não instalar um importador extra apenas para visualizar a concept.

Mapas BaseColor devem usar a configuração de cor apropriada; normal deve ser importado como Normal Map. Máscaras são dados, com canais definidos pelo shader/material efetivamente usado. Não presumir que uma máscara de outro pipeline atende ao URP. Verificar transparência do Guardião, emissão das bobinas e materiais do cabelo na cena mais cara.

Dois materiais no herói e três nos chefes são metas iniciais de slots por conjunto; cada renderer/submesh pode gerar custo mesmo com material compartilhado. Contar instâncias visíveis, sombras, skinning e overdraw. Texel 2K RGBA8 tem cerca de 16 MiB sem compressão e sem mips; a cadeia de mips adiciona aproximadamente um terço. Não usar o tamanho do PNG em disco como estimativa de VRAM.

## LOD e teste de deformação

LOD0 é próximo; LOD1 reduz geometria secundária; LOD2 preserva cabeça, acessórios de identificação e ameaça. Não somar como ativos simultâneos os níveis alternativos; contabilizar custo transitório de crossfade quando usado. Distâncias/tamanho em tela são ajustados na câmera real após medir, não herdados de um template de jogo 3D em terceira pessoa.

Teste obrigatório: agachar, correr, levantar os braços, alcançar ferramenta, virar e receber impacto. Verificar ombro/quadril, mãos, joelhos e colisões do poncho. Para Faísca, curl/roll/dig; Trovico, asa aberta/fechada e pouso; máquinas, limite de cada pistão/anéis e exposição de fraqueza. Revisar também bounds do SkinnedMeshRenderer para não desaparecer em uma pose extrema.

Fonte oficial sobre tipos de rig: https://docs.unity3d.com/6000.3/Documentation/Manual/FBXImporter-Rig.html . As escolhas por personagem e budgets são decisões deste projeto, não garantias universais da Unity.
