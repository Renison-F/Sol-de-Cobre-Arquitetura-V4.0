# Incrementos A–F (execução headless) — resposta de implementação

Branch: `feat/incrementos-a-d-headless`. Trabalho sobre o kit `unity-project-template/`
(o código que se copia para `Game/Assets`). Seguindo `13-handoff-claude-codex.md`:
mudanças pequenas e rastreáveis, sem segredos no cliente, sem marcar backlog produtivo
como concluído por haver só texto ou C# não homologado no Unity.

## Ambiente desta execução

Container Linux **sem Unity/IL2CPP**. Para dar evidência real, a camada **Domain**
(engine-free, só `System.*`) foi compilada e testada com **Roslyn (.NET SDK 8, C# 12)**
fora do projeto Unity: **44/44 testes EditMode passam**. Runtime/Editor/UI usam
`UnityEngine*` e **só compilam no Unity** — foram escritos e revisados, não compilados aqui.

## A — bootstrap

- **Arquivos:** `unity-project-template/.gitignore`, `.gitattributes`, `Packages/README.md`;
  `Domain/Core/GamePhase.cs` (estendido) + `Tests/EditMode/GamePhaseTransitionTests.cs`.
- **Funciona (testado):** `GamePhaseMachine` agora cobre `Loading→Menu` (recuperação),
  `Playing→Menu` e `Paused→Menu` (encerrar sessão), com `CancelLoading()`/`EndSession()`.
- **Motivo:** DEV-02 pedia recuperação/cancelamento/encerramento; faltavam config Git e a
  lista explícita de pacotes para o setup local.
- **Limite:** não fabriquei `manifest.json`/lock/`ProjectVersion.txt` — o Editor os gera
  (regra "não adivinhar versões"). Seguir `08-bootstrap-local-v4.md` + `Packages/README.md`.

## B — sensação (input, touch, câmera)

- **Arquivos:** `Runtime/Gameplay/GameplayControls.cs`, `PlayerInputBridge.cs` (reescrito),
  `Domain/Gameplay/AxisProcessing.cs` (+testes), `Presentation/CameraFollow2_5D.cs`,
  novo assembly `UI/` (`TouchHoldButton.cs`, `TouchHud.cs`, `README.md`), `ProjectBootstrap.cs`.
- **Funciona (testado):** `AxisProcessing` (deadzone radial 1D do direcional virtual).
- **Funciona (revisado, homologar no Unity):** mapa único de ações teclado/gamepad/touch;
  ponte com eventos (Interact/Ability/Switch/Pause) que **zera eixos e solta habilidade
  presa ao perder foco/pausar**; `TouchHoldButton` solta ao sair do botão (pointer próprio);
  câmera com dead zone 1,2 m, look-ahead 2 m, amortecimento 0,18 s, perspectiva FOV 35°.
- **Limite:** o canvas de toque é montado no editor (ver `UI/README.md`), não gerado por
  script (evitar cena não compilada). Dois dedos/30-60 FPS/orientação: validar em aparelho.

## C — interação (laço, Faísca, cancelamento)

- **Arquivos (Domain, testado):** `TargetSelection.cs`, `AbilityController.cs`,
  `CompanionDispatch.cs` (+`AbilityTests.cs`). **Runtime (revisado):** `AbilityTargetSource.cs`,
  `AbilityTargeting.cs`, `LassoAbility.cs`, `AbilityDriver.cs`, `World/CompanionController.cs`.
- **Funciona (testado):** seleção por prioridade (missão no cone > central > ID);
  habilidade sem alvo = windup curto **sem gasto nem cooldown**; cancelar não deixa
  efeito/dano; Faísca ação 1,5 s + retorno 2 s e **sempre volta ao dono**, inclusive cancelada.
- **Limite:** física de balanço/cabo, raycast e apresentação do laço são trabalho de cena.

## D — persistência (schema3)

- **Arquivos (Domain, testado):** `SaveDataV3.cs`, `SaveValidationV3.cs`, `FlagCatalog.cs`
  (60 flags de `campanha.json`), `SaveMigrator.cs`, `ISaveCodec.cs`, `SaveStore.cs`
  (+`SaveMigrationTests.cs`, `SaveStoreTests.cs`). **Runtime:** `UnityJsonSaveCodec.cs`,
  `SaveSystem.cs`. **Fixtures:** `11-contratos/fixtures/`.
- **Funciona (testado):** migração 1→3 campo a campo (flags conhecidas preservadas,
  desconhecidas só no relatório); validação completa do contrato; store com 3 slots,
  gravação atômica (tmp+flush+promoção com backup), recuperação de corrupção
  (backup → preservação), **versão futura preservada, nunca sobrescrita**.
- **Limite:** o I/O de produção usa `JsonUtility`/`persistentDataPath` (codec testado por
  um equivalente engine-free); validar no Editor/aparelho. Casos de suspensão em
  ataque/cutscene (restauração sem hitbox) dependem da integração em cena.

## E — encontro (duas famílias + Fiscal)

- **Arquivos (Domain, testado — 16 testes novos):** `Narrative/MissionProgress.cs`,
  `Gameplay/EnemyThreatCycle.cs`, `Gameplay/BossEncounter.cs`, `Gameplay/HeroVitality.cs`
  (+`EncounterTests.cs`). **Runtime (revisado):** `World/FiscalBoss.cs`, `World/CommonEnemy.cs`,
  `Narrative/MissionController.cs`.
- **Funciona (testado):**
  - **Fiscal (BOSS-01):** `Dormant→Intro→Telegraph→Active→Recover→Exposed→Reposition`;
    **3 exposições** (Carimbo tel1.2/0.3/2.5 e Varredura tel1.4/0.6/1.8), um acerto válido
    por janela, **blindado fora da janela** (feedback sem dano), avanço por **timer/deadline**
    (não espera callback), **pausa congela timers**, **checkpoint restaura a fase atual**
    mantendo exposições concluídas, e **derrota/vitória idempotentes** (publica BOSS_01_DEFEATED
    uma vez; reentrada após vitória não reconta).
  - **Inimigo comum:** ameaça Prepare/Active/Recover, dano só em Active, recuo mínimo 1,5 s,
    pausa não dispara ataque, assistência +35% no aviso, derrota publica ID uma vez.
  - **Missão M01 ordenada:** flags em ordem; fora de ordem rejeitada; evento repetido/atrasado
    é no-op seguro; recompensa concedida uma única vez; restauração por prefixo do save.
  - **Vida do herói:** 5 unidades, dano 1 (ou 0,5 com assistência), morte/reset.
- **Limite:** arena, hitboxes espaciais, blocos de carga, clipes de animação e movimento de
  dive/turret são trabalho de cena; `CommonEnemy` traz patrol/hover. Validar em 30/60 FPS,
  bordas, callback de animação ausente e reentrada após vitória (QA de BOSS-01.md) no Editor.

## F — apresentação (HUD, áudio, leitura de ameaça; aceite do trio)

F tem duas metades: **leitura da ameaça** (lógica de HUD/áudio — feita e testada aqui) e
**aceite dos assets** do trio (arte 3D — critério aqui; arquivos no seu PC).

- **Arquivos (Domain, testado — 9 testes novos):** `Hud/ThreatPresentation.cs`,
  `Hud/PlayerPreferences.cs`, `Hud/InputDeviceTracker.cs`, `Hud/AudioCueBus.cs`,
  `Hud/HudModel.cs` (+`HudTests.cs`). **Runtime:** `Hud/AudioDirector.cs`,
  `UI/HudController.cs`, `UI/AccessibilityController.cs`. **Doc:** `09-arte-audio/aceite-blockout-trio.md`.
- **Funciona (testado):**
  - **Leitura de ameaça em perfil baixo:** ameaça por **forma + som**, nunca só cor; **flash
    desligado em "reduzir flashes"** sem perder a leitura; fases do Fiscal/inimigo mapeiam
    para aviso/ativo.
  - **Áudio:** prioridade do pool (aviso de dano > diálogo > interação > ambiente > detalhe);
    aviso continua audível com a música desligada (canais separados).
  - **Preferências por perfil:** clamp de escala/opacidade/legenda; as três opções do save
    (assist/subtitles/reduced_flashes) fazem round-trip com SaveOptions.
  - **Glifo de dispositivo:** rastreia o último usado (teclado/gamepad/touch).
  - **HudModel:** compõe vida/objetivo/glifo/ameaça para a view.
- **Aceite de assets:** `09-arte-audio/aceite-blockout-trio.md` lista alvos do trio
  (de `modelagem.json`) e o checklist no aparelho. O validador existente passa
  (`tools/test_model_delivery.py`: 12/12).
- **Limite:** malhas/rigs/texturas/animações/áudio do trio precisam de Blender/Unity e
  ferramentas de arte na sua máquina (PROD-01). O canvas da HUD é montado no editor.

## Como homologar localmente (resumo)

1. Criar projeto URP no Unity **6000.3.24f1**; copiar `_Game` e os arquivos Git (A).
2. Adicionar `com.unity.inputsystem` e `com.unity.test-framework` (ver `Packages/README.md`).
3. **Test Runner → EditMode**: devem passar os testes de Domain (os mesmos 44 do harness).
4. Gerar o laboratório (`Sol de Cobre → Bootstrap`) e validar andar/saltar/câmera.
5. Montar o HUD de toque (ver `Assets/_Game/UI/README.md`) e validar touch em aparelho.

## Próximo passo e pré-condição

Depois do gate acima: produção de arte do trio (PROD-01, no Unity/Blender) e o recorte do
**slice de 8–12 min** da Estação (Incremento H / `09-ordem-de-implementacao.md`). Em paralelo,
**G (acesso comercial)** depende de contas/sandbox reais do proprietário (não fabricáveis aqui).
Pré-condição: Editor compilando o kit e EditMode verde (os mesmos testes do harness).
