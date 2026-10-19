# Como transformar as pranchas em personagens de jogo

As imagens definem aparência e linguagem visual; elas não contêm profundidade recuperável de forma única, malha, esqueleto ou animação. O modelo final precisa preservar a identidade em movimento e no enquadramento lateral. Gerar uma forma 3D a partir da imagem pode acelerar exploração, mas não dispensa retopologia, UV, rig e revisão de direitos/origem da ferramenta usada.

## O que cada termo produz

| Etapa | O que se faz | Arquivo/prova | Critério para avançar |
|---|---|---|---|
| Turnaround técnico | Alinhar frente, perfil e costas em ortográfica; resolver acessórios contraditórios | Prancha com altura, eixos, pose e vista de mãos/solas | Mesmas proporções e peças nas vistas |
| Blockout | Construir volumes simples em escala | Fonte Blender e preview cinza | Silhueta legível na câmera de gameplay; mãos alcançam alvos |
| Escultura | Refinar rosto, roupas e materiais onde a leitura pede | High poly, se necessário ao bake | Identidade aprovada antes de detalhes de superfície |
| Retopologia | Refazer malha eficiente e deformável | Low poly com loops de ombro/cotovelo/joelho/face | Sem esticamento crítico nas poses de teste |
| UV | Abrir a malha para receber textura | UV principal, ilhas e padding para mipmaps | Sem vazamento de bake; espelhamentos documentados |
| Bake | Transferir relevo do high poly ao low poly | Normal e mapas de apoio | Sem costura/reflexo quebrado em luz giratória |
| Materiais | Definir tecido, couro, cobre e emissão | Mapas e material URP de referência | Cor e relevo legíveis sem depender de render cinematográfico |
| Rig | Construir controles e esqueleto | Armature e controles no .blend | Escala e eixos estáveis; rig respeita família do personagem |
| Skinning | Distribuir influência dos ossos na malha | Pesos e teste de deformação | Sem vértices soltos, colapsos ou placas de metal elásticas |
| Animação | Fazer movimento e atuação | Clips in-place e take de revisão | Antecipação, contato e recuperação legíveis |
| LOD | Reduzir custo conforme tamanho na tela | Três níveis e transições | Silhueta preservada; sem estalo ou material ausente |
| Integração | Montar visual, Animator, sockets e colisores | Prefab Unity | Lógica funciona com troca de visual e materiais |
| Homologação | Medir em cena e aparelho | Manifesto, vídeo e captura de profiler | Cumpre orçamento da arena, não apenas de um ator isolado |

## Primeiro trio: Mara, Faísca e Fiscal

Produzir primeiro Mara em blockout, Faísca com um teste de enrolar/escavar e Fiscal com carimbo/trava funcionando. Não detalhar 17 modelos simultaneamente antes de validar esse trio. O objetivo é descobrir cedo se o poncho cobre a mão do laço, se Faísca cabe no túnel e se a trava do Fiscal é alcançável pela câmera e pelo alvo lógico.

No blockout, medir salto e plataforma usando o collider, não a aba do chapéu. O rosto precisa ser reconhecível em menus e cinematografia, mas a leitura durante plataforma vem de silhueta, pose e contraste. Aumentar resolução de textura não resolve um braço escondido por tecido. Só fechar escultura/UV após o teste lateral na cena.

## Famílias de produção

Humanos podem compartilhar convenção de esqueleto, biblioteca de poses e materiais; cada um mantém rosto e proporções próprios. Faísca e Trovico usam rigs Generic distintos. Máquinas usam peças rígidas com pivôs e poucos deformadores. Cascavel usa módulos e cadeia/spline; a Máquina da Seca é conjunto modular com colisão de plataformas separada da aparência. Reutilizar tecnologia não significa copiar o mesmo corpo/rosto entre personagens.

As 17 fichas em `09-arte-audio/modelagem/INDICE.md` explicam as diferenças e dão budgets específicos. Não pedir ao modelador apenas “fazer igual à imagem”; entregar também pose de referência, escala proposta, câmera, ações, sockets e aceite.
