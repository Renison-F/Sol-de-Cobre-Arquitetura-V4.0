# Aceite de blockout do trio — Mara, Faísca, Fiscal (Incremento F)

Checklist de aceite para o trecho da Estação. **Fonte numérica única:** `11-contratos/modelagem.json`.
Modelos **ainda não produzidos** — este documento é o critério, não uma declaração de entrega.
Validar o manifesto de cada entrega com `tools/validate_model_delivery.py <manifesto> --require-accepted`.

## Alvos (de modelagem.json)

| Asset | asset_id | Rig | LOD0/1/2 (tri) | Ossos | Mat. | Altura | Clipes-chave |
|---|---|---|---|---|---|---|---|
| Mara Vento | CHR-MARA-VENTO | Humanoid | 18000/9000/4500 | 70 | 2 | 1,68 m | idle, run, jump_*, lasso_cast/hold/release, hurt, rescue |
| Faísca | CHR-FAISCA | Generic | 8000/4000/2000 | 42 | 2 | 0,62 m | idle_sniff, walk/run, dig_enter/loop/exit, point, puddle_startle |
| Fiscal de Trilhos | BOSS-01 | Generic | 20000/10000/5000 | 40 | 3 | 2,4 m | intro, telegraph_A/B, attack_A/B, recover_A/B, exposed, defeated |

Sockets obrigatórios (para o código já existente): Mara `belt_lasso`/`companion_anchor`;
Faísca `dig_origin`/`ground_contact`; Fiscal `stamp_origin`/`dorsal_latch`.

## Leitura da ameaça em perfil baixo (código já pronto e testado)

A lógica de leitura de ameaça está em `SolDeCobre.Hud.ThreatPresentation` (testada):
- Ameaça comunicada por **forma** (`ShapeId`) **e som** (`PlaySound`), **nunca só por cor**.
- **Flash desligado** em "reduzir flashes" sem perder a leitura (forma+som permanecem).
- Fases do Fiscal/inimigo mapeiam para aviso/ativo (`telegraph`/`Prepare` → Warning; `attack`/`Active` → Active).

Os clipes de arte devem casar com essas janelas: a silhueta de `telegraph_A/B` do Fiscal
precisa ser legível em LOD2/perfil baixo e distinta de `attack_A/B`, com áudio de aviso
próprio (prioridade `DamageWarning` em `AudioCueBus`). Em ≤ 1 s o jogador distingue
perigo, cenário e plataforma (meta de UX de `hud-acessibilidade.md`).

## Aceite no aparelho (manual, no seu PC)

1. Blockout importado no Unity, escala conferida contra `height_m` e o herói do capítulo.
2. `tools/validate_model_delivery.py` passa para os três manifestos (arquivos .blend/.fbx reais,
   LOD0/1/2, orçamentos de triângulo/ossos/materiais dentro do contrato).
3. Deformação revisada (sem interpenetração grosseira do poncho de Mara/carapaça da Faísca).
4. Transições de LOD sem "pop" perceptível no enquadramento 2.5D da câmera.
5. **Perfil gráfico baixo**: telegraph vs attack do Fiscal continuam legíveis por forma+som;
   plataformas e aterrissagem visíveis sem flash intenso.
6. HUD legível a 720p com safe area; glifos trocam conforme o dispositivo usado.

> Limite real: malhas, rigs, texturas, animações e áudio precisam de Blender/Unity e das
> ferramentas de arte/áudio na sua máquina. Este kit entrega a **lógica** de HUD/ameaça/áudio
> (verificada) e o **critério** de aceite; os arquivos de arte são produção no backlog (PROD-01).
