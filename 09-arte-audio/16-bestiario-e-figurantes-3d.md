# Produção 3D de inimigos comuns e figurantes

Além das 17 fichas individuais, as três pranchas do bestiário representam **19 variantes** e a prancha de figurantes representa **três famílias**. Não confundir quatro PNGs coletivos com quatro únicos modelos. O plano abaixo cobre os 22 pacotes secundários; metas em `11-contratos/modelagem-secundarios.json`, ainda sem modelos produzidos.

As quatro famílias de IA não obrigam uma única anatomia: patrol inclui pedra-viva, bípede e caranguejo, que podem compartilhar comportamento e precisam de rig próprio. Reutilizar controladores, materiais e módulos de máquina; não deformar uma malha humana para representar um caranguejo.

| ID / nome | LOD0/1/2 | Bones máx. | Tratamento específico |
|---|---|---|---|
| ENM-01 · Operário-drone | 5000/2500/1250 | 20 | Alça de manutenção destacada do corpo. |
| ENM-02 · Morcego de caldeira | 6000/3000/1500 | 32 | Asas mecânicas e caldeira separadas. |
| ENM-03 · Ecomorcego | 6000/3000/1500 | 32 | Morcego orgânico estilizado com equipamento de eco; não copiar malha robótica sem adaptar. |
| ENM-04 · Pedra-viva | 8000/4000/2000 | 36 | Pedras podem ser rígidas com core separado; weakpoint abre após rolagem. |
| ENM-05 · Mineiro de lata | 8000/4000/2000 | 36 | Picareta e mão com socket; não soldar ferramenta ao torso. |
| ENM-06 · Sentinela de carvão | 7000/3500/1750 | 20 | Respiro/jato como peça e VFX distintos. |
| ENM-07 · Lampião autônomo | 5000/2500/1250 | 20 | Vidro do lampião com overdraw limitado; alça rígida. |
| ENM-08 · Espantalho de aço | 8000/4000/2000 | 36 | Membros finos e roupa por poucos auxiliares, sem cloth obrigatório. |
| ENM-09 · Guarda de acoplamento | 8000/4000/2000 | 36 | Acoplador e mãos separados; contato de aterramento legível. |
| ENM-10 · Drone-pantógrafo | 5000/2500/1250 | 20 | Pantógrafo abre com pivôs rígidos. |
| ENM-11 · Caranguejo ferrífero | 8000/4000/2000 | 36 | Pinça, válvula e pernas articuladas sem malha de metal elástica. |
| ENM-12 · Sentinela hidráulica | 7000/3500/1750 | 20 | Reservatório/console com leitura simples de pressão. |
| ENM-13 · Balão-sentinela | 5000/2500/1250 | 20 | Envelope opaco, hélices/lastro em peças distintas. |
| ENM-14 · Coruja de cobre | 6000/3000/1500 | 32 | Asas em grupos de placas; lente/olhos legíveis na distância de mergulho. |
| ENM-15 · Projeção guardiã | 5000/2500/1250 | 20 | Figura projetada e base como peças independentes; não exige novo corpo humano deformável complexo. |
| ENM-16 · Sentinela de lente | 7000/3500/1750 | 20 | Base, tripé e lente em pivôs; feixe como VFX externo. |
| ENM-17 · Sentinela de cobre | 8000/4000/2000 | 36 | Escudo abre por pivô, mantendo hitbox de gameplay independente. |
| ENM-18 · Torre de vapor | 7000/3500/1750 | 20 | Válvula e chaminés separados; vapor por VFX com teto. |
| ENM-19 · Sentinela da Aurora | 5000/2500/1250 | 20 | Anéis/core com pivôs e dois modos de resposta visual. |
| NPC-01 · Ferroviários | 10000/5000/2500 | 55 | Base humana compartilhável; variações de cabeça/roupa e props de trabalho. |
| NPC-02 · Jardineiros e operadores de água | 10000/5000/2500 | 55 | Base humana compartilhável; variações de cabeça/roupa e props de trabalho. |
| NPC-03 · Técnicos da Cidadela | 10000/5000/2500 | 55 | Base humana compartilhável; variações de cabeça/roupa e props de trabalho. |

## Implementação e aceite

Inimigos mantêm antecipação, ataque, recuperação e resposta definidas em bestiario.json; os clips não alteram os tempos autorais por conta própria. Pool reinicializa estado, sockets, emissão, partículas e hitboxes. Antes de reutilizar um inimigo, nenhum handler antigo ou alvo de laço pode sobreviver ao despawn. Variação de cor não basta para comunicar ataque diferente.

Figurantes compartilham esqueleto e clipes de trabalho, com contato ajustado por prop; máximo de oito animados no perfil médio segue o orçamento global. Reduzir quantidade/LOD no perfil baixo. Não bloquear o corredor do jogador com crowd e não transformar NPC em alvo de combate. Origem/direitos e visual sem estereótipos seguem o guia já existente.

Até dois slots materiais por conjunto e mapas principais 1K como baseline. Todas as instâncias visíveis contam no orçamento da cena. Medir seis inimigos e oito figurantes junto de herói, mascote, efeitos e cenário, sem somar limites individuais como se fossem orçamento garantido do aparelho. Alturas e colisores são fechados no blockout de cada fase e registrados no aceite.

O mesmo contrato de handoff pode ser validado com --catalog apontando para 11-contratos/modelagem-secundarios.json. Não preencher métricas antes da exportação/importação real. A ordem continua: trio do slice, depois famílias comuns necessárias à Estação, e expansão por fase.
