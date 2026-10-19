# Pranchas V3.2 — prompts, procedência e legendas

Registro histórico: na V3.3 o nome passou a Isadora Veredas. Os prompts verbatim abaixo conservam o nome usado nas gerações anteriores; os caminhos do acervo apontam para os arquivos atuais.

Produzidas e editadas com a ferramenta nativa de geração de imagens. São conceitos rasterizados, não malhas, rigs ou desenhos técnicos de engenharia. Os PNGs finais estão em `09-assets/personagens`; os caminhos completos aparecem abaixo.

## Convenção de detalhes

Todas as 17 pranchas individuais têm legenda em cada quadro pequeno de detalhes. As legendas descrevem o recorte, em português, caixa alta, tipografia serifada marrom, centralizada em faixa clara de areia. Referência de tratamento: Madame Engrenagem desta revisão. Não misturar quadros de detalhes sem legenda com quadros legendados. Preservar os recortes e a quantidade de detalhes de cada personagem. Bestiários e figurantes são pranchas coletivas identificadas por ENM/NPC; a imagem promocional da dupla não possui faixa de detalhes.

| Prancha | Legendas da esquerda para a direita | Arquivo final |
|---|---|---|
| Mara Vento | ROSTO · FIVELA · CÁPSULA · CORDA · PONCHO · BOTA | `09-assets/personagens/mara-vento.png` |
| Zé Trovão | ROSTO · CHAPÉU · FIVELA · CÁPSULA · PONCHO | `09-assets/personagens/ze-trovao.png` |
| Faísca | ROSTO · LENÇO · CÁPSULA · MANTA · MOCHILA · CARAPAÇA | `09-assets/personagens/faisca-tatu.png` |
| Trovico | ROSTO · MEDALHÃO · CÁPSULA · LENÇO · PLUMAGEM | `09-assets/personagens/trovico-falcao.png` |
| Isadora Veredas | ROSTO · ASTROLÁBIO · DIÁRIO · BOLSA · CASACO | `09-assets/personagens/isadora-veredas.png` |
| Cascavel de Silício | CABEÇA · SEGMENTOS · CHOCALHO | `09-assets/personagens/cascavel-de-silicio.png` |
| Guardião dos Canais | ROSTO · RESERVATÓRIO · ARTICULAÇÃO | `09-assets/personagens/guardiao-dos-canais.png` |
| Luna Fagulha | ROSTO · PISTOLA · FIVELA · FERRAMENTAS · PONCHO · BOTA | `09-assets/personagens/luna-fagulha.png` |
| Barão Ferrugem | ROSTO · BENGALA · PRÓTESE · RESERVATÓRIO · CASACO | `09-assets/personagens/barao-ferrugem.png` |
| Fiscal de Trilhos | VISOR · CARIMBO · PISTÕES · BILHETE · BLINDAGEM | `09-assets/personagens/fiscal-de-trilhos.png` |
| Britador Colosso | VISOR · BROCA · ESTEIRA · NÚCLEO · BLINDAGEM | `09-assets/personagens/britador-colosso.png` |
| Maquinista Sem Rosto | MÁSCARA · NÚCLEO · GARRA · CHAVE · VÁLVULA | `09-assets/personagens/maquinista-sem-rosto.png` |
| Condutor Relâmpago | VISOR · PINÇA · PANTÓGRAFO · BOTA · MEDALHÃO | `09-assets/personagens/condutor-relampago.png` |
| General Para-Raios | CABEÇA · BOBINA · MANOPLA · OMBRO · PISTÃO | `09-assets/personagens/general-para-raios.png` |
| Oráculo de Latão | ÍRIS · ANEL · ESFERA · JUNTA · GRAVAÇÃO | `09-assets/personagens/oraculo-de-latao.png` |
| Madame Engrenagem | ROSTO · BROCHE · DRONE · BASTÃO DE COMANDO · CASACO E ARNÊS | `09-assets/personagens/madame-engrenagem.png` |
| Máquina da Seca | NÚCLEO · TANQUE · MANÔMETRO · JUNTA · PLATAFORMA | `09-assets/personagens/maquina-da-seca.png` |

## Etapa 1 — dez pranchas refeitas

Em cada geração, a imagem 1 foi a prancha original do próprio sujeito no pacote V3.1, preservando identidade, e a imagem 2 foi Mara Vento V3.1, usada apenas para linguagem visual, materiais e composição. Os prompts abaixo registram essa etapa. A etapa 2 aplica a revisão final de legendas; não reutilizar a saída intermediária sem legendas. Madame já contém as legendas e é a referência dessa revisão.

### Luna Fagulha

```text
Use case: style-transfer and identity-preserve. Rebuild Image 1 as a LANDSCAPE 1536x1024 production character sheet in precisely the visual language and layout of Image 2 (Mara Vento V3.1). Image 1 is the edit target and identity reference. Image 2 is the composition/materials/stylization reference ONLY; do not copy Mara's face or clothing. LOCK identity, species, age, silhouette, color palette, costume and equipment of Image 1. Match the refined expressive stylized 3D rendering, sculpted volume, worn leather/brushed copper, warm golden studio lighting and soft sandstone canyon background of Image 2. REQUIRED LAYOUT: one large full-body three-quarter hero view in left 36%; three smaller COMPLETE-body neutral front, TRUE SIDE PROFILE and back views across upper right; a neatly aligned five-panel detail strip across bottom right (face/core, signature mechanism, material, accessory, construction detail). Serif uppercase name at top right with thin ornamental divider. Every view must depict the same equipment and proportions. All feet, wheels, antennae and accessories fully visible with margins. Do not use a white painted-circle background, do not omit the bottom detail strip, no action-pose collage, no watermark. These are visual concept sheets, not geometry exports. Subject: Preserve Luna's youthful adult female inventor identity, copper-red braided hair, expressive green eyes, big leather cowboy hat with goggles, rust-red patterned poncho, cream cropped work shirt, leather utility harness and belt, rugged brown trousers and boots, copper and blue handheld repair instrument from the original. She is a distinct red-haired engineer, not Mara. Show tool detailing in strip; consistent tool and costume across views. Text (verbatim): "LUNA FAGULHA".
```

### Barão Ferrugem

```text
Use case: style-transfer and identity-preserve. Rebuild Image 1 as a LANDSCAPE 1536x1024 production character sheet in precisely the visual language and layout of Image 2 (Mara Vento V3.1). Image 1 is the edit target and identity reference. Image 2 is the composition/materials/stylization reference ONLY; do not copy Mara's face or clothing. LOCK identity, species, age, silhouette, color palette, costume and equipment of Image 1. Match the refined expressive stylized 3D rendering, sculpted volume, worn leather/brushed copper, warm golden studio lighting and soft sandstone canyon background of Image 2. REQUIRED LAYOUT: one large full-body three-quarter hero view in left 36%; three smaller COMPLETE-body neutral front, TRUE SIDE PROFILE and back views across upper right; a neatly aligned five-panel detail strip across bottom right (face/core, signature mechanism, material, accessory, construction detail). Serif uppercase name at top right with thin ornamental divider. Every view must depict the same equipment and proportions. All feet, wheels, antennae and accessories fully visible with margins. Do not use a white painted-circle background, do not omit the bottom detail strip, no action-pose collage, no watermark. These are visual concept sheets, not geometry exports. Subject: Preserve the older male industrial antagonist, silver swept hair, short angular silver beard, tall black western hat, long tailored black coat with bronze trim, brass mechanical gauntlet, pressure-gauge cane and amber reservoir on back. Give him restrained stylized sculpted facial expressiveness consistent with Mara's world; preserve his age and sophisticated severe personality. Detail strip: face, cane gauge, gauntlet, amber tank, coat embroidery. Text (verbatim): "BARÃO FERRUGEM".
```

### Fiscal de Trilhos

```text
Use case: style-transfer and identity-preserve. Rebuild Image 1 as a LANDSCAPE 1536x1024 production character sheet in precisely the visual language and layout of Image 2 (Mara Vento V3.1). Image 1 is the edit target and identity reference. Image 2 is the composition/materials/stylization reference ONLY; do not copy Mara's face or clothing. LOCK identity, species, age, silhouette, color palette, costume and equipment of Image 1. Match the refined expressive stylized 3D rendering, sculpted volume, worn leather/brushed copper, warm golden studio lighting and soft sandstone canyon background of Image 2. REQUIRED LAYOUT: one large full-body three-quarter hero view in left 36%; three smaller COMPLETE-body neutral front, TRUE SIDE PROFILE and back views across upper right; a neatly aligned five-panel detail strip across bottom right (face/core, signature mechanism, material, accessory, construction detail). Serif uppercase name at top right with thin ornamental divider. Every view must depict the same equipment and proportions. All feet, wheels, antennae and accessories fully visible with margins. Do not use a white painted-circle background, do not omit the bottom detail strip, no action-pose collage, no watermark. These are visual concept sheets, not geometry exports. Subject: Preserve the squat copper railway inspector automaton, bolted station hat, amber eye slit, broad rounded metal torso, enormous red/cream striped stamping arm, ticket-punch hand and heavy wheel-like boots. Show functional piston and stamp mechanism; fully mechanical, no human skin. Detail strip: eye/hat, stamp face, pistons, ticket clamp, weathered copper plating. Text (verbatim): "FISCAL DE TRILHOS".
```

### Britador Colosso

```text
Use case: style-transfer and identity-preserve. Rebuild Image 1 as a LANDSCAPE 1536x1024 production character sheet in precisely the visual language and layout of Image 2 (Mara Vento V3.1). Image 1 is the edit target and identity reference. Image 2 is the composition/materials/stylization reference ONLY; do not copy Mara's face or clothing. LOCK identity, species, age, silhouette, color palette, costume and equipment of Image 1. Match the refined expressive stylized 3D rendering, sculpted volume, worn leather/brushed copper, warm golden studio lighting and soft sandstone canyon background of Image 2. REQUIRED LAYOUT: one large full-body three-quarter hero view in left 36%; three smaller COMPLETE-body neutral front, TRUE SIDE PROFILE and back views across upper right; a neatly aligned five-panel detail strip across bottom right (face/core, signature mechanism, material, accessory, construction detail). Serif uppercase name at top right with thin ornamental divider. Every view must depict the same equipment and proportions. All feet, wheels, antennae and accessories fully visible with margins. Do not use a white painted-circle background, do not omit the bottom detail strip, no action-pose collage, no watermark. These are visual concept sheets, not geometry exports. Subject: Preserve the massive mining automaton on wide caterpillar tracks, huge conical drill arm on the same side as original, other clamp arm, hunched copper furnace torso, orange/charcoal hazard markings, single amber eye and blue dorsal cooling vent. Complete front/side/back machine views aligned to ground; no clipping of drill or tracks. Detail strip: eye, drill, tread, cooling vent, worn hazard armor. Text (verbatim): "BRITADOR COLOSSO".
```

### Maquinista Sem Rosto

```text
undefined Subject: Preserve the tall slender nonhuman abandoned shunting exoskeleton: smooth featureless black visor, navy conductor cap and long distressed navy coat, bronze skeletal cable arms and metal legs, large railway-key tool, blue lantern chamber in chest, circular wheel mechanism on back. It has NO eyes or human face behind the visor. Detail strip: blank visor, chest lantern, cable hand, railway key, coat and back mechanism. Family-friendly mysterious atmosphere, not horror. Text (verbatim): "MAQUINISTA SEM ROSTO".
```

### Condutor Relâmpago

```text
undefined Subject: Preserve the sleek copper conductor automaton with narrow face plate and two cobalt eyes, navy cap with brass lightning emblem, tailored navy split-tail coat, horseshoe electromagnets at both wrists, folded pantograph frame on back and insulated boots. No skin or human anatomy replacing metal parts. Detail strip: eye/cap, wrist magnet, pantograph, insulated foot, copper coat fastener. Text (verbatim): "CONDUTOR RELÂMPAGO".
```

### General Para-Raios

```text
undefined Subject: Preserve the massive inverted-triangle storm automaton: broad bronze pauldrons, deep burgundy military-style long coat, high dark cap with lightning-rod finial, glowing amber eyes, huge gauntlets, chunky piston boots and three tall blue induction coils on its back. No real-world insignia or firearms. Detail strip: head, coil, gauntlet, shoulder, piston foot. Keep rear coils consistent in every view. Text (verbatim): "GENERAL PARA-RAIOS".
```

### Oráculo de Latão

```text
undefined Subject: Preserve the sentient spherical brass astronomical computer: one large sapphire lens-eye, concentric astrolabe rings, engraved star maps, small orbital spheres and three articulated tripod legs. Entirely nonhuman machine, not a character in clothes. True side view must clearly show depth of rings; back shows engraved circular cover, front shows lens. Detail strip: iris lens, gear/ring, orbital sphere, tripod joint, star-map brass engraving. Text (verbatim): "ORÁCULO DE LATÃO".
```

### Madame Engrenagem

```text
undefined Subject: Preserve the mature dark-skinned female industrial commander with short swept silver-gray curls, intelligent composed expression, burgundy western riding coat, cream blouse, bronze engineering harness and belt, dark trousers and boots, cog brooch, copper command baton and two small shoulder-level copper drones. Keep her age and dignified stance. Detail strip: face, gear brooch, drone, baton, coat/harness; no borrowed face or costume from Mara. Text (verbatim): "MADAME ENGRENAGEM".
```

### Máquina da Seca

```text
undefined Subject: Preserve the colossal climate machine: blue storm heart inside concentric copper rings, amber pressure tanks, two side platform balconies, riveted copper structure on four articulated support legs, small pilot balcony below the central dial, cables and banners. No extra humanoid characters. Render the ENTIRE machine fitting each front/side/back view without cropping or changing its mechanical silhouette. Detail strip: storm core, amber tank, pressure dial, articulated leg joint, platform/cable connection. Hero view can be scaled to fit left column while preserving proportions; add no new hardware. Text (verbatim): "MÁQUINA DA SECA".
```

## Etapa 2 — padronização final de legendas

Imagem 1: a prancha do sujeito; para sete pranchas anteriores, a versão V3.1, e para nove das dez refeitas, a saída da etapa 1. Imagem 2: Madame Engrenagem da etapa 1, referência apenas de tratamento das legendas. Cada prompt mantém a identidade e altera a faixa de detalhes. Madame permanece como a saída da etapa 1.

### Mara Vento

```text
Use case: text-localization. Asset: existing game character concept sheet. Image 1 is the edit target; Image 2 is ONLY the caption design reference. Make ONE precise change to Image 1: add a matching warm parchment caption band directly underneath EVERY existing bottom small detail panel, like Image 2. Keep the current number/order/width of panels (do not force five); preserve every crop, main character, face, costume, title, turnaround views, lighting, colors and overall landscape 1536x1024 layout. Fit the caption bands inside the existing lower margin; if necessary slightly shorten ONLY detail panels to create room. Captions dark brown, uppercase serif, centered, SAME consistent approximate 15–17px type size, thin copper border, pale sandstone fill. All labels fully readable, no clipping, no missing accents. Do not add labels under turnaround views. Do not alter the anatomy, equipment or other image content. Labels from left to right, verbatim: "ROSTO", "FIVELA", "CÁPSULA", "CORDA", "PONCHO", "BOTA".
```

### Zé Trovão

```text
Use case: text-localization. Asset: existing game character concept sheet. Image 1 is the edit target; Image 2 is ONLY the caption design reference. Make ONE precise change to Image 1: add a matching warm parchment caption band directly underneath EVERY existing bottom small detail panel, like Image 2. Keep the current number/order/width of panels (do not force five); preserve every crop, main character, face, costume, title, turnaround views, lighting, colors and overall landscape 1536x1024 layout. Fit the caption bands inside the existing lower margin; if necessary slightly shorten ONLY detail panels to create room. Captions dark brown, uppercase serif, centered, SAME consistent approximate 15–17px type size, thin copper border, pale sandstone fill. All labels fully readable, no clipping, no missing accents. Do not add labels under turnaround views. Do not alter the anatomy, equipment or other image content. Labels from left to right, verbatim: "ROSTO", "CHAPÉU", "FIVELA", "CÁPSULA", "PONCHO".
```

### Faísca

```text
Use case: text-localization. Asset: existing game character concept sheet. Image 1 is the edit target; Image 2 is ONLY the caption design reference. Make ONE precise change to Image 1: add a matching warm parchment caption band directly underneath EVERY existing bottom small detail panel, like Image 2. Keep the current number/order/width of panels (do not force five); preserve every crop, main character, face, costume, title, turnaround views, lighting, colors and overall landscape 1536x1024 layout. Fit the caption bands inside the existing lower margin; if necessary slightly shorten ONLY detail panels to create room. Captions dark brown, uppercase serif, centered, SAME consistent approximate 15–17px type size, thin copper border, pale sandstone fill. All labels fully readable, no clipping, no missing accents. Do not add labels under turnaround views. Do not alter the anatomy, equipment or other image content. Labels from left to right, verbatim: "ROSTO", "LENÇO", "CÁPSULA", "MANTA", "MOCHILA", "CARAPAÇA".
```

### Trovico

```text
Use case: text-localization. Asset: existing game character concept sheet. Image 1 is the edit target; Image 2 is ONLY the caption design reference. Make ONE precise change to Image 1: add a matching warm parchment caption band directly underneath EVERY existing bottom small detail panel, like Image 2. Keep the current number/order/width of panels (do not force five); preserve every crop, main character, face, costume, title, turnaround views, lighting, colors and overall landscape 1536x1024 layout. Fit the caption bands inside the existing lower margin; if necessary slightly shorten ONLY detail panels to create room. Captions dark brown, uppercase serif, centered, SAME consistent approximate 15–17px type size, thin copper border, pale sandstone fill. All labels fully readable, no clipping, no missing accents. Do not add labels under turnaround views. Do not alter the anatomy, equipment or other image content. Labels from left to right, verbatim: "ROSTO", "MEDALHÃO", "CÁPSULA", "LENÇO", "PLUMAGEM".
```

### Isadora Veredas

```text
Use case: text-localization. Asset: existing game character concept sheet. Image 1 is the edit target; Image 2 is ONLY the caption design reference. Make ONE precise change to Image 1: add a matching warm parchment caption band directly underneath EVERY existing bottom small detail panel, like Image 2. Keep the current number/order/width of panels (do not force five); preserve every crop, main character, face, costume, title, turnaround views, lighting, colors and overall landscape 1536x1024 layout. Fit the caption bands inside the existing lower margin; if necessary slightly shorten ONLY detail panels to create room. Captions dark brown, uppercase serif, centered, SAME consistent approximate 15–17px type size, thin copper border, pale sandstone fill. All labels fully readable, no clipping, no missing accents. Do not add labels under turnaround views. Do not alter the anatomy, equipment or other image content. Labels from left to right, verbatim: "ROSTO", "ASTROLÁBIO", "DIÁRIO", "BOLSA", "CASACO".
```

### Cascavel de Silício

```text
Use case: text-localization. Asset: existing game character concept sheet. Image 1 is the edit target; Image 2 is ONLY the caption design reference. Make ONE precise change to Image 1: add a matching warm parchment caption band directly underneath EVERY existing bottom small detail panel, like Image 2. Keep the current number/order/width of panels (do not force five); preserve every crop, main character, face, costume, title, turnaround views, lighting, colors and overall landscape 1536x1024 layout. Fit the caption bands inside the existing lower margin; if necessary slightly shorten ONLY detail panels to create room. Captions dark brown, uppercase serif, centered, SAME consistent approximate 15–17px type size, thin copper border, pale sandstone fill. All labels fully readable, no clipping, no missing accents. Do not add labels under turnaround views. Do not alter the anatomy, equipment or other image content. Labels from left to right, verbatim: "CABEÇA", "SEGMENTOS", "CHOCALHO".
```

### Guardião dos Canais

```text
Use case: text-localization. Asset: existing game character concept sheet. Image 1 is the edit target; Image 2 is ONLY the caption design reference. Make ONE precise change to Image 1: add a matching warm parchment caption band directly underneath EVERY existing bottom small detail panel, like Image 2. Keep the current number/order/width of panels (do not force five); preserve every crop, main character, face, costume, title, turnaround views, lighting, colors and overall landscape 1536x1024 layout. Fit the caption bands inside the existing lower margin; if necessary slightly shorten ONLY detail panels to create room. Captions dark brown, uppercase serif, centered, SAME consistent approximate 15–17px type size, thin copper border, pale sandstone fill. All labels fully readable, no clipping, no missing accents. Do not add labels under turnaround views. Do not alter the anatomy, equipment or other image content. Labels from left to right, verbatim: "ROSTO", "RESERVATÓRIO", "ARTICULAÇÃO".
```

### Luna Fagulha

```text
Use case: text-localization. Asset: existing game character concept sheet. Image 1 is the edit target; Image 2 is ONLY the caption design reference. Make ONE precise change to Image 1: add a matching warm parchment caption band directly underneath EVERY existing bottom small detail panel, like Image 2. Keep the current number/order/width of panels (do not force five); preserve every crop, main character, face, costume, title, turnaround views, lighting, colors and overall landscape 1536x1024 layout. Fit the caption bands inside the existing lower margin; if necessary slightly shorten ONLY detail panels to create room. Captions dark brown, uppercase serif, centered, SAME consistent approximate 15–17px type size, thin copper border, pale sandstone fill. All labels fully readable, no clipping, no missing accents. Do not add labels under turnaround views. Do not alter the anatomy, equipment or other image content. Labels from left to right, verbatim: "ROSTO", "PISTOLA", "FIVELA", "FERRAMENTAS", "PONCHO", "BOTA".
```

### Barão Ferrugem

```text
Use case: text-localization. Asset: existing game character concept sheet. Image 1 is the edit target; Image 2 is ONLY the caption design reference. Make ONE precise change to Image 1: add a matching warm parchment caption band directly underneath EVERY existing bottom small detail panel, like Image 2. Keep the current number/order/width of panels (do not force five); preserve every crop, main character, face, costume, title, turnaround views, lighting, colors and overall landscape 1536x1024 layout. Fit the caption bands inside the existing lower margin; if necessary slightly shorten ONLY detail panels to create room. Captions dark brown, uppercase serif, centered, SAME consistent approximate 15–17px type size, thin copper border, pale sandstone fill. All labels fully readable, no clipping, no missing accents. Do not add labels under turnaround views. Do not alter the anatomy, equipment or other image content. Labels from left to right, verbatim: "ROSTO", "BENGALA", "PRÓTESE", "RESERVATÓRIO", "CASACO".
```

### Fiscal de Trilhos

```text
Use case: text-localization. Asset: existing game character concept sheet. Image 1 is the edit target; Image 2 is ONLY the caption design reference. Make ONE precise change to Image 1: add a matching warm parchment caption band directly underneath EVERY existing bottom small detail panel, like Image 2. Keep the current number/order/width of panels (do not force five); preserve every crop, main character, face, costume, title, turnaround views, lighting, colors and overall landscape 1536x1024 layout. Fit the caption bands inside the existing lower margin; if necessary slightly shorten ONLY detail panels to create room. Captions dark brown, uppercase serif, centered, SAME consistent approximate 15–17px type size, thin copper border, pale sandstone fill. All labels fully readable, no clipping, no missing accents. Do not add labels under turnaround views. Do not alter the anatomy, equipment or other image content. Labels from left to right, verbatim: "VISOR", "CARIMBO", "PISTÕES", "BILHETE", "BLINDAGEM".
```

### Britador Colosso

```text
Use case: text-localization. Asset: existing game character concept sheet. Image 1 is the edit target; Image 2 is ONLY the caption design reference. Make ONE precise change to Image 1: add a matching warm parchment caption band directly underneath EVERY existing bottom small detail panel, like Image 2. Keep the current number/order/width of panels (do not force five); preserve every crop, main character, face, costume, title, turnaround views, lighting, colors and overall landscape 1536x1024 layout. Fit the caption bands inside the existing lower margin; if necessary slightly shorten ONLY detail panels to create room. Captions dark brown, uppercase serif, centered, SAME consistent approximate 15–17px type size, thin copper border, pale sandstone fill. All labels fully readable, no clipping, no missing accents. Do not add labels under turnaround views. Do not alter the anatomy, equipment or other image content. Labels from left to right, verbatim: "VISOR", "BROCA", "ESTEIRA", "NÚCLEO", "BLINDAGEM".
```

### Maquinista Sem Rosto

```text
Use case: text-localization. Asset: existing game character concept sheet. Image 1 is the edit target; Image 2 is ONLY the caption design reference. Make ONE precise change to Image 1: add a matching warm parchment caption band directly underneath EVERY existing bottom small detail panel, like Image 2. Keep the current number/order/width of panels (do not force five); preserve every crop, main character, face, costume, title, turnaround views, lighting, colors and overall landscape 1536x1024 layout. Fit the caption bands inside the existing lower margin; if necessary slightly shorten ONLY detail panels to create room. Captions dark brown, uppercase serif, centered, SAME consistent approximate 15–17px type size, thin copper border, pale sandstone fill. All labels fully readable, no clipping, no missing accents. Do not add labels under turnaround views. Do not alter the anatomy, equipment or other image content. Labels from left to right, verbatim: "MÁSCARA", "NÚCLEO", "GARRA", "CHAVE", "VÁLVULA".
```

### Condutor Relâmpago

```text
Use case: text-localization. Asset: existing game character concept sheet. Image 1 is the edit target; Image 2 is ONLY the caption design reference. Make ONE precise change to Image 1: add a matching warm parchment caption band directly underneath EVERY existing bottom small detail panel, like Image 2. Keep the current number/order/width of panels (do not force five); preserve every crop, main character, face, costume, title, turnaround views, lighting, colors and overall landscape 1536x1024 layout. Fit the caption bands inside the existing lower margin; if necessary slightly shorten ONLY detail panels to create room. Captions dark brown, uppercase serif, centered, SAME consistent approximate 15–17px type size, thin copper border, pale sandstone fill. All labels fully readable, no clipping, no missing accents. Do not add labels under turnaround views. Do not alter the anatomy, equipment or other image content. Labels from left to right, verbatim: "VISOR", "PINÇA", "PANTÓGRAFO", "BOTA", "MEDALHÃO".
```

### General Para-Raios

```text
Use case: text-localization. Asset: existing game character concept sheet. Image 1 is the edit target; Image 2 is ONLY the caption design reference. Make ONE precise change to Image 1: replace the existing detail caption bands with a matching warm parchment caption band directly underneath EVERY existing bottom small detail panel, like Image 2. Keep the current number/order/width of panels (do not force five); preserve every crop, main character, face, costume, title, turnaround views, lighting, colors and overall landscape 1536x1024 layout. Fit the caption bands inside the existing lower margin; if necessary slightly shorten ONLY detail panels to create room. Captions dark brown, uppercase serif, centered, SAME consistent approximate 15–17px type size, thin copper border, pale sandstone fill. All labels fully readable, no clipping, no missing accents. Keep any existing labels under turnaround views unchanged; do not add new ones. Do not alter the anatomy, equipment or other image content. Labels from left to right, verbatim: "CABEÇA", "BOBINA", "MANOPLA", "OMBRO", "PISTÃO". Replace existing detail texts, no duplicate caption row. Match Image 2's regular Roman serif lettering, not heavy Western lettering.
```

### Oráculo de Latão

```text
Use case: text-localization. Asset: existing game character concept sheet. Image 1 is the edit target; Image 2 is ONLY the caption design reference. Make ONE precise change to Image 1: replace the existing detail caption bands with a matching warm parchment caption band directly underneath EVERY existing bottom small detail panel, like Image 2. Keep the current number/order/width of panels (do not force five); preserve every crop, main character, face, costume, title, turnaround views, lighting, colors and overall landscape 1536x1024 layout. Fit the caption bands inside the existing lower margin; if necessary slightly shorten ONLY detail panels to create room. Captions dark brown, uppercase serif, centered, SAME consistent approximate 15–17px type size, thin copper border, pale sandstone fill. All labels fully readable, no clipping, no missing accents. Keep any existing labels under turnaround views unchanged; do not add new ones. Do not alter the anatomy, equipment or other image content. Labels from left to right, verbatim: "ÍRIS", "ANEL", "ESFERA", "JUNTA", "GRAVAÇÃO". Replace existing detail texts, no duplicate caption row. Match Image 2's regular Roman serif lettering, not heavy Western lettering.
```

### Máquina da Seca

```text
Use case: text-localization. Asset: existing game character concept sheet. Image 1 is the edit target; Image 2 is ONLY the caption design reference. Make ONE precise change to Image 1: replace the existing detail caption bands with a matching warm parchment caption band directly underneath EVERY existing bottom small detail panel, like Image 2. Keep the current number/order/width of panels (do not force five); preserve every crop, main character, face, costume, title, turnaround views, lighting, colors and overall landscape 1536x1024 layout. Fit the caption bands inside the existing lower margin; if necessary slightly shorten ONLY detail panels to create room. Captions dark brown, uppercase serif, centered, SAME consistent approximate 15–17px type size, thin copper border, pale sandstone fill. All labels fully readable, no clipping, no missing accents. Keep any existing labels under turnaround views unchanged; do not add new ones. Do not alter the anatomy, equipment or other image content. Labels from left to right, verbatim: "NÚCLEO", "TANQUE", "MANÔMETRO", "JUNTA", "PLATAFORMA". Replace existing detail texts, no duplicate caption row. Match Image 2's regular Roman serif lettering, not heavy Western lettering.
```
