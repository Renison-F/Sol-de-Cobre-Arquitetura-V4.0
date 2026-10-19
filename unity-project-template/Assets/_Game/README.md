# Incrementos A–D (headless) adicionados

Ver `12-desenvolvimento/15-incrementos-a-d-headless.md`. Resumo do que entrou:
- **Domain** (engine-free, testado com Roslyn, 44 testes EditMode): `GamePhase` estendido;
  `Gameplay/AxisProcessing`, `TargetSelection`, `AbilityController`, `CompanionDispatch`;
  `Progression/SaveDataV3`, `SaveValidationV3`, `FlagCatalog`, `SaveMigrator`, `ISaveCodec`, `SaveStore`.
- **Runtime:** `GameplayControls`, `PlayerInputBridge` (reescrito), `CameraFollow2_5D`,
  `AbilityTargetSource/Targeting`, `LassoAbility`, `AbilityDriver`, `World/CompanionController`,
  `Progression/UnityJsonSaveCodec`, `SaveSystem`.
- **UI** (novo assembly `SolDeCobre.UI`): `TouchHoldButton`, `TouchHud` (ver `UI/README.md`).
- Config Git (`.gitignore`/`.gitattributes`) e `Packages/README.md` na raiz do template.

Runtime/Editor/UI só compilam no Unity; Domain foi verificado fora dele. Migrar por diff.

---

# Kit de referência Unity V4

Quatro asmdefs, Domain sem UnityEngine, Runtime com Input System, Editor com ProjectBootstrap e quatro testes EditMode. Guia local: `12-desenvolvimento/08-bootstrap-local-v4.md`. O gerador cria assets reais quando executado no Unity; não foi executado neste ambiente. Não copiar esta _Game junto da antiga: migrar por diff para evitar classes duplicadas. QuestState.Ready agora recebe flags; QuestRules adapta QuestDefinition.

# Referências C# V3

Este diretório contém scripts de referência, sem cenas, prefabs, Package Manager ou ProjectSettings de um projeto Unity completo. Copiar para um projeto URP real na versão homologada e compilar antes de integrar. Não houve compilação Unity/IL2CPP nesta entrega.

Mudanças V3: motor com coyote time, buffer, salto variável e controle de plano; persistência que distingue corrupção, backup e versão sem apagar silenciosamente dados; política e verificador criptográfico SC1 com interfaces de bridge, storage e transporte.

`RsaLicenseVerifier` verifica algoritmo fixo, chave pública, assinatura e gramática canônica de claims. `LicensePolicy` trata prazo e âncora de tempo. Instanciar com chaves públicas confiáveis, app/plataforma exatos e lista de revogação autenticada; nenhuma chave privada deve entrar no cliente. Testar provider RSA em IL2CPP e compatibilidade com os vetores Python. A suíte Python não compila C# nem prova interoperabilidade do binário.

O save C# continua **schema 1 de compatibilidade** para exemplificar I/O seguro. O formato alvo V3 em `11-contratos/save.schema.json` requer novo DTO e migração de flags. `Read` agora devolve status e out snapshot; nenhum chamador deve tratar `InvalidPreserved` como campanha nova sem escolha explícita do usuário. Se `File.Replace` não for suportado no provider, Write retorna false preservando arquivo anterior; criar adaptador nativo com promoção atômica e teste de falha para essa plataforma.

Os adaptadores de loja/KMS/rede não estão implementados. Isso é item concreto do backlog, sem mocks silenciosos de sucesso ou TODOs que escondam bloqueio comercial.
