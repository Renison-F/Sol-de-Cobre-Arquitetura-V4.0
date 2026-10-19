# Incrementos A–D (execução headless) — resposta de implementação

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

## Como homologar localmente (resumo)

1. Criar projeto URP no Unity **6000.3.24f1**; copiar `_Game` e os arquivos Git (A).
2. Adicionar `com.unity.inputsystem` e `com.unity.test-framework` (ver `Packages/README.md`).
3. **Test Runner → EditMode**: devem passar os testes de Domain (os mesmos 44 do harness).
4. Gerar o laboratório (`Sol de Cobre → Bootstrap`) e validar andar/saltar/câmera.
5. Montar o HUD de toque (ver `Assets/_Game/UI/README.md`) e validar touch em aparelho.

## Próximo passo e pré-condição

Depois do gate acima: Incremento E (encontro com o Fiscal) e o recorte do slice da Estação
(`09-ordem-de-implementacao.md`). Pré-condição: Editor compilando o kit e EditMode verde.
