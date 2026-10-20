# Pacotes do projeto — resolução no Editor

Este kit **não fornece** `manifest.json`, `packages-lock.json` nem `ProjectVersion.txt`.
Esses arquivos são gerados pelo Unity na sua máquina e devem ser versionados **reais**,
não chutados a partir do número do Editor (ver `12-desenvolvimento/08-bootstrap-local-v4.md`,
regra "não adivinhar versões de pacotes").

## Como obter os pacotes certos

1. Criar o projeto **Universal 3D / URP** no Unity Hub com a baseline **6000.3.24f1**.
   O template URP já traz `com.unity.render-pipelines.universal` resolvido.
2. Abrir **Window → Package Manager** e instalar, nas versões que o Editor oferecer como
   compatíveis:
   - `com.unity.inputsystem` — Input System (touch/teclado/gamepad).
   - `com.unity.test-framework` — Unity Test Framework (testes EditMode deste kit).
3. Em **Edit → Project Settings → Player → Active Input Handling**, selecionar
   **Input System Package (New)** (ou *Both* durante a transição) e reiniciar se pedido.
4. Depois que o Editor resolver tudo, **commitar** `Packages/manifest.json`,
   `Packages/packages-lock.json` e `ProjectSettings/ProjectVersion.txt` gerados.

## Dependências que o código deste kit assume

| Assembly do kit | Pacote necessário | Namespace usado |
|---|---|---|
| `SolDeCobre.Runtime` | `com.unity.inputsystem` | `UnityEngine.InputSystem`, `UnityEngine.InputSystem.OnScreen` |
| `SolDeCobre.Runtime` | URP (render pipeline) | materiais/shaders `Universal Render Pipeline/*` |
| `SolDeCobre.Editor` | `com.unity.inputsystem` | geração de Input Actions no laboratório |
| `SolDeCobre.Tests.EditMode` | `com.unity.test-framework` | `NUnit.Framework` |

`SolDeCobre.Domain` não referencia UnityEngine e compila sem pacotes de engine.
