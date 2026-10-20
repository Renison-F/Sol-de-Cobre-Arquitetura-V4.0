# HUD de toque — montagem no editor

Os componentes deste assembly (`SolDeCobre.UI`) sintetizam o input touch sobre os
mesmos caminhos de gamepad do mapa `Gameplay` (ver `Runtime/Gameplay/GameplayControls.cs`),
então teclado, gamepad e toque passam pelas **mesmas ações** — nenhuma regra de jogo
olha código de tecla.

> Esta pasta fornece os **scripts**; o canvas em si é montado no editor (uGUI) e validado
> em dispositivo. Não há geração automática do canvas para não entregar cena não compilada.
>
> **HUD de jogo** (vida, objetivo, ameaça) alimentado pelo `GameDirector`: ver `HUD-CANVAS.md`.
> Este documento cobre o **HUD de toque** (direcional + botões).

## Passos no Unity

1. **Canvas**: criar `UI → Canvas`. Render Mode `Screen Space - Overlay`, `Canvas Scaler`
   em *Scale With Screen Size* (referência landscape, ex. 1920×1080), *Match* 0,5.
2. **EventSystem**: ao criar o Canvas, trocar o módulo por **Input System UI Input Module**
   (o Input System oferece o botão "Replace with InputSystemUIInputModule").
3. **Painel de safe area**: um `RectTransform` filho (stretch total) para abrigar os clusters.
4. **Cluster de movimento** (padrão: lado esquerdo): um `RectTransform` com o
   **On-Screen Stick** (do Input System) cujo *Control Path* aponte para
   `GameplayControls.TouchStickPath` (`<Gamepad>/leftStick`). Raio inicial ~72 dp, zona
   morta 0,15 (a deadzone final é aplicada por `AxisProcessing` na ponte).
5. **Cluster de ação** (padrão: lado direito): quatro botões usando `TouchHoldButton`,
   cada um com o *Control Path* correspondente:
   - Salto → `TouchJumpPath` (`<Gamepad>/buttonSouth`), ~64 dp
   - Habilidade → `TouchAbilityPath` (`<Gamepad>/buttonWest`), ~56 dp
   - Interação/mascote → `TouchInteractPath` (`<Gamepad>/buttonNorth`), ~56 dp
   - Trocar herói → `TouchSwitchPath` (`<Gamepad>/rightShoulder`), ~48 dp
   Distância mínima 12 dp entre botões; área interativa ≥ 48 dp.
6. **TouchHud**: adicionar ao Canvas, ligar `group` (CanvasGroup), `safeAreaPanel`,
   `moveCluster`, `actionCluster`. Expõe opacidade (50–100%), escala (80–140%), canhoto
   e `RestoreLayout()` para o botão restaurar.

## Garantias já codificadas

- `TouchHoldButton` solta a ação ao **levantar o dedo ou arrastar para fora** do botão,
  e cada botão rastreia seu próprio pointer (habilidade não rouba o salto).
- `PlayerInputBridge` zera eixos e dispara `AbilityReleased` ao perder foco/pausar, então
  nenhum comando fica preso; o pause do SO e a perda de foco são tratados separadamente.
- `TouchHud.ApplySafeArea()` reaplica a safe area quando `Screen.safeArea` muda.

## A validar em dispositivo (não automatizável aqui)

Dois dedos simultâneos (mover + saltar), três dedos, toque na borda, 30/60 FPS, chamada do
sistema, e troca/bloqueio de orientação. Ver `05-game-design/controles-touch.md`.
