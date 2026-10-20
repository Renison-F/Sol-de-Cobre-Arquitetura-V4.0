# Montagem do canvas da HUD (passo a passo)

A lógica de HUD já está pronta e testada (`SolDeCobre.Hud`): `GameDirector` monta um
`HudSnapshot` por frame e chama `HudController.SetSnapshot`. Este guia é só a **montagem
visual no editor** — o canvas não é gerado por script para não entregar cena não compilada.

> Para o **HUD de toque** (direcional + botões), veja `UI/README.md`. Este documento é o
> **HUD de jogo** (vida, objetivo, ameaça) que o `GameDirector` alimenta.

## Fluxo de dados

```
HeroVitality ─┐
MissionProgress (NextObjective) ─┤
FiscalBoss.Phase → ThreatPresentation ─┤→ HudModel.Build → HudSnapshot → HudController.SetSnapshot → UI
InputDeviceTracker (GlyphSet) ─┤
PlayerPreferences (ReducedFlashes/Subtitles) ─┘
```

`HudController` desenha; **a fonte da verdade é o jogo**. A view nunca decide regra.

---

## 1. Canvas base

1. `GameObject → UI → Canvas`. Nele:
   - **Render Mode:** `Screen Space - Overlay`.
   - **Canvas Scaler:** `UI Scale Mode = Scale With Screen Size`; `Reference Resolution 1920×1080`;
     `Screen Match Mode = Match Width Or Height`, `Match = 0.5`. (Garante legibilidade a 720p.)
2. Filho `SafeArea` (`RectTransform` esticado, âncoras 0,0–1,1). Opcional: reaproveite o
   `TouchHud.ApplySafeArea()` apontando `safeAreaPanel` para este painel; ou um script de
   safe area próprio. Todos os widgets abaixo ficam dentro de `SafeArea`.

## 2. Vida (corações) — campo `hearts` (Image[])

1. Dentro de `SafeArea`, um `HorizontalLayoutGroup` no canto superior esquerdo (bússola/vida).
2. Crie **5** `UI → Image` (um por unidade de vida padrão). Em cada um:
   - `Image Type = Filled`, `Fill Method = Horizontal` (ou Radial), `Fill Origin` à esquerda.
   - Um sprite de coração/pip com **forma clara** (contraste alto; não dependa só de cor).
3. No `HudController`, arraste os 5 Images para o array **`hearts`**, na ordem esquerda→direita.
   - O controller faz `hearts[i].fillAmount = clamp01(HeartsCurrent - i)`:
     vida 5 → 5 cheios; vida 3,5 → 3 cheios + 1 meio + 1 vazio (suporta o dano 0,5 da assistência).

## 3. Próximo objetivo — campo `objectiveLabel` (Text)

1. `UI → Text` (ou TextMeshPro, trocando o tipo no `HudController` se preferir TMP) discreto,
   no topo/centro. Fonte com fallback de acentos; teste expansão de ~40%.
2. Arraste para **`objectiveLabel`**. O texto vem de `MissionController.NextObjective`
   (ex.: "Examinar o poço") — **ação concreta, sem exigir leitura de IDs**.
3. A dica contextual é opt-in e **não pode cobrir o botão de salto** (posicione acima/ao lado).

## 4. Indicador de ameaça — campos `threatShape`, `threatFlash`, `threatGroup`

A ameaça é comunicada por **forma + som**, e o flash é só reforço (desligado em "reduzir flashes").

1. Um `GameObject` `ThreatShape` com um ícone de **alerta por forma** (ex.: losango/seta),
   distinto da forma de **impacto**. Arraste para **`threatShape`** — o controller faz
   `SetActive(Threat.Active)`.
2. Opcional: um `Graphic` (Image) `ThreatFlash` para o reforço de flash. Arraste para
   **`threatFlash`** — só fica `enabled` quando `Threat.Active && Threat.Flash` (ou seja,
   **nunca** quando "reduzir flashes" está ligado).
3. Um `CanvasGroup` no container da ameaça, arrastado para **`threatGroup`** — o controller
   ajusta `alpha` por `Threat.Intensity` (aviso ~0,6; golpe ativo ~1,0), some quando inativo.
4. **Áudio:** o som do aviso é tocado pelo `AudioDirector` (cue `DamageWarning`, canal SFX),
   audível mesmo com a música desligada. Associe o clipe no `AudioDirector.clips`.

## 5. Glifo de dispositivo e legendas (pontos de extensão)

O `HudSnapshot` carrega `GlyphSet` ("glyphs_keyboard/gamepad/touch") e `Subtitles`, mas o
`HudController` enxuto não troca glifos sozinho. Para ativar:
- **Glifos:** num script seu, leia `snapshot.GlyphSet` e troque o conjunto de sprites dos
  prompts; atualize o `InputDeviceTracker` do `GameDirector` quando o dispositivo mudar
  (callback do Input System). *(extensão — o dado já existe no snapshot.)*
- **Legendas:** um painel de legenda cuja visibilidade segue `snapshot.Subtitles` e cujo
  tamanho segue `PlayerPreferences.SubtitleScale`.

## 6. Ligar o `GameDirector`

Num `GameObject` `GameDirector` (ou no gerado pelo slice), preencha:

| Campo | Arraste |
|---|---|
| `hud` | o objeto com `HudController` |
| `mission` | o `MissionController` |
| `boss` | o `FiscalBoss` (se a cena tiver o chefe) |
| `accessibility` | o `AccessibilityController` (opcional, para flashes/escala/volumes) |
| `input` | o `PlayerInputBridge` do player (para a pausa) |
| `enemies` | os `CommonEnemy` da seção (para congelar na pausa) |
| `slot` | 1–3 (slot de save desta sessão) |

Para iniciar: chame `GameDirector.StartSession()` (ex.: botão "Continuar/Novo jogo" do menu).
Ele lê o save do slot, restaura a sessão e faz `Menu → Loading → Playing`. Save inválido/
versão futura retorna `false` e deve levar à **tela de recuperação** (telas-e-fluxos.md).

## 7. Checklist de aceite (hud-acessibilidade.md / 12-desenvolvimento/02)

- [ ] Legível a **720p** e dentro da **safe area** real do aparelho.
- [ ] Vida, objetivo e ameaça distinguíveis em **menos de 1 s**.
- [ ] Ameaça reconhecível **por forma e som** com cores/flap desligados.
- [ ] "Reduzir flashes" tira o flash **sem** perder a leitura da ameaça.
- [ ] Dica não cobre o salto; foco navegável por gamepad/teclado sem trap.
- [ ] Texto com fallback de fonte e espaço para expansão de ~40%.
