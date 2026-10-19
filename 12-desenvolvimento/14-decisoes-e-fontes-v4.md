# Decisões consolidadas e fontes técnicas — V4

Consulta em 19/10/2026. A baseline do pacote continua Unity 6000.3.24f1/URP, C#, física 3D no plano XY e Input System. Não houve seleção automática de versão mais nova nem homologação de requisitos de loja. Manifest/pacotes/SDKs são resolvidos no projeto real e seus locks precisam ser versionados.

| Tema | Decisão do projeto | Estado |
|---|---|---|
| Engine | Baseline verificável existente, sem atualizar no meio do bootstrap | Editor/lock a homologar localmente |
| Geometria | Blender fonte; FBX para Unity; GLB só preview opcional | Pipeline e métricas por identidade especificados |
| Rig | Humanoid para humanos, Generic para mascotes/máquinas | A mapear e testar em cada modelo |
| Camadas | Domain puro, Runtime, Editor e EditMode agora; Application quando houver casos de uso | Assemblies e referência entregues; sem compilação Unity aqui |
| Save | Schema3 é contrato alvo; exemplo legado continua explicitamente schema1 | Implementação/migração DEV-08/09 |
| Automação | Scene/prefab/input/lab Android por scripts do Editor | Código incluído, execução local necessária |
| Plataformas | Android primeiro incremento técnico; iOS exige etapa Xcode/macOS | Não há APK/IPA validado neste ZIP |

## Correção da interpretação de capacidade

Uma sessão sem Editor instalado não consegue compilar Unity apenas por escrever C#. Isso não significa que toda cena/prefab precise ser montada manualmente: o Editor oferece APIs para gerar assets e executar métodos em lote. Blender também admite execução em background/scripts. Automatizar arquivos não substitui direção de arte, revisão de rig, teste de sensação ou homologação em celular.

## Fontes oficiais

- [Release baseline 6000.3.24f1](https://unity.com/releases/editor/whats-new/6000.3.24f1).
- [PrefabUtility.SaveAsPrefabAsset](https://docs.unity3d.com/6000.3/Documentation/ScriptReference/PrefabUtility.SaveAsPrefabAsset.html).
- [BuildPipeline e customização de build](https://docs.unity3d.com/6000.3/Documentation/Manual/BuildPlayerPipeline.html).
- [Argumentos do Editor e executeMethod](https://docs.unity3d.com/6000.3/Documentation/Manual/EditorCommandLineArguments.html).
- [Build por linha de comando](https://docs.unity3d.com/6000.3/Documentation/Manual/build-command-line.html).
- [Rig e Generic/Humanoid](https://docs.unity3d.com/6000.3/Documentation/Manual/FBXImporter-Rig.html).
- [API de montagem de ações](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.17/api/UnityEngine.InputSystem.InputActionSetupExtensions.html).
- [Input Actions — referência consultada, não lock de versão](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.17/manual/Actions.html).
- [Build iOS em duas etapas](https://docs.unity3d.com/6000.3/Documentation/Manual/iphone-BuildProcess.html).
- [Blender CLI/background](https://docs.blender.org/manual/en/latest/advanced/command_line/arguments.html).

As metas numéricas, decisões de modelo e critérios de gameplay desta V4 são propostas de engenharia/arte do projeto. Não são requisitos oficiais universais dessas ferramentas.
