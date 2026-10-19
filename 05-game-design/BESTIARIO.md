# Bestiário completo — 19 variantes

Quatro famílias compartilham lógica e têm configurações e visuais distintos. `patrol`: patrulha segmento com detecção de borda, prepara golpe corpo a corpo e recua. `hover`: flutua em faixa limitada, anuncia trajetória/ação e volta ao ponto de origem. `dive`: espera alvo, congela direção no aviso e mergulha sem seguir jogador instantaneamente. `turret`: imóvel, anuncia feixe/jato e resfria. Dormant/Despawn e Defeated são ciclo de vida; a ameaça tem no máximo Prepare/Active/Recover.

| ID | Variante | Nível | Família | Integridade | Aviso s | Ativo s | Recuperação s | Resposta | Prancha |
|---|---|---|---|---|---|---|---|---|---|
| ENM-01 | Operário-drone | 1 | hover | 1 | 1.0 | 0,5 | 1,5 | puxar alça no recuo | `bestiario-01.png` |
| ENM-02 | Morcego de caldeira | 1 | dive | 1 | 1.0 | 0,5 | 1,5 | esperar mergulho e esquivar | `bestiario-01.png` |
| ENM-03 | Ecomorcego | 2 | dive | 1 | 1.0 | 0,5 | 1,5 | usar sino para romper pulso | `bestiario-01.png` |
| ENM-04 | Pedra-viva | 2 | patrol | 2 | 1.0 | 0,5 | 1,5 | induzir rolagem contra parede e puxar cristal exposto com laço | `bestiario-01.png` |
| ENM-05 | Mineiro de lata | 3 | patrol | 1 | 1.0 | 0,5 | 1,5 | pular ataque e usar laço | `bestiario-01.png` |
| ENM-06 | Sentinela de carvão | 3 | turret | 2 | 1.2 | 0,5 | 1,5 | desviar jato e puxar trava do respiro | `bestiario-01.png` |
| ENM-07 | Lampião autônomo | 4 | hover | 1 | 1.0 | 0,5 | 1,5 | acender farol na pausa | `bestiario-02.png` |
| ENM-08 | Espantalho de aço | 4 | patrol | 2 | 1.0 | 0,5 | 1,5 | laço solta braço preso | `bestiario-02.png` |
| ENM-09 | Guarda de acoplamento | 5 | patrol | 2 | 1.0 | 0,5 | 1,5 | usar placa de aterramento | `bestiario-02.png` |
| ENM-10 | Drone-pantógrafo | 5 | hover | 1 | 1.0 | 0,5 | 1,5 | esperar arco terminar | `bestiario-02.png` |
| ENM-11 | Caranguejo ferrífero | 6 | patrol | 1 | 1.0 | 0,5 | 1,5 | saltar pinça e puxar válvula | `bestiario-02.png` |
| ENM-12 | Sentinela hidráulica | 6 | turret | 2 | 1.2 | 0,5 | 1,5 | equalizar pressão no console | `bestiario-02.png` |
| ENM-13 | Balão-sentinela | 7 | hover | 1 | 1.0 | 0,5 | 1,5 | puxar lastro marcado | `bestiario-03.png` |
| ENM-14 | Coruja de cobre | 7 | dive | 1 | 1.0 | 0,5 | 1,5 | esquivar mergulho e acionar haste | `bestiario-03.png` |
| ENM-15 | Projeção guardiã | 8 | hover | 1 | 1.0 | 0,5 | 1,5 | desligar base projetora | `bestiario-03.png` |
| ENM-16 | Sentinela de lente | 8 | turret | 1 | 1.2 | 0,5 | 1,5 | refletir feixe no sensor | `bestiario-03.png` |
| ENM-17 | Sentinela de cobre | 9 | patrol | 2 | 1.0 | 0,5 | 1,5 | impacto após defesa aberta | `bestiario-03.png` |
| ENM-18 | Torre de vapor | 9 | turret | 2 | 1.2 | 0,5 | 1,5 | girar válvula após vapor | `bestiario-03.png` |
| ENM-19 | Sentinela da Aurora | 10 | hover | 2 | 1.0 | 0,5 | 1,5 | alternar laço e aterramento | `bestiario-03.png` |

## Regras compartilhadas

Dano 1, teto de seis inimigos comuns ativos na janela da câmera; chefes não somam seis adicionais sem revisão de budget. Agressão exige linha de visão e raio de 8 m, patrulha não cai de plataformas salvo encontro explicitamente desenhado. Recuo mínimo oferece 1,5 s entre ações. Aviso tem áudio e forma; não depender de brilho azul. Assistência aumenta aviso 35%.

Derrota desativa dano, publica ID uma vez e devolve ao pool depois do efeito. Animais orgânicos fogem; máquinas desmontam em peças sem violência gráfica. Carregamento e pausa não disparam ataques imediatamente. Morte do herói reseta inimigos da seção a estado neutro e não duplica coletável persistente.

## Arte e animação

As pranchas usam rótulo ENM por célula. Não são sprites prontos, atlases UV ou geometrias. Cada variante recebe blockout, malha 4k–8k triângulos LOD0, até 2 materiais, textura 1K, clips idle/locomotion/prepare/active/recover/defeat e prefab com collider validado. Família aérea tem teto de altitude e retorno seguro. Variantes mecânicas podem compartilhar rig quando proporções permitirem.

## Aceite

Um teste de encontro por variante comprova alcance, aviso, dano, recuperação, resposta e reset. Nenhum spawn aparece sobre herói ou em pouso obrigatório. Validar todos com áudio desligado e todos com VFX reduzido. Aparência diferente não significa 19 árvores de IA copiadas.

Ataque básico de Mara é puxar trava/argola exposta com laço, também em inimigos; não usa arma de fogo. Zé usa impacto nos alvos marcados. Pedra-viva aparece antes de Zé: sua resposta obrigatória é induzir colisão na parede e puxar cristal, não exigir impacto ainda indisponível.
