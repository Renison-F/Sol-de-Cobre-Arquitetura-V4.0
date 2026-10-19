# Fixtures de save por versão (regressão)

Entradas para testar migração e recuperação do save (DEV-08/09/11, QA-02). Antes do
primeiro release, ampliar com saves reais exportados de cada versão publicada.

| Arquivo | Versão | Resultado esperado ao carregar |
|---|---|---|
| `../save.example.json` | v3 válido | `Loaded` |
| `save-v1-legacy.json` | v1 legado | `Migrated` → v3 equivalente, flags conhecidas preservadas |
| `save-v3-future.json` | v4 (futuro) | `UnsupportedVersion` — preservado, nunca sobrescrito |

## Nota de formato do save legado (v1)

O **código** legado (`SaveSnapshot` / `LocalSaveRepository`) usa o schema 1 com
`schemaVersion`, `chapterId`, `checkpointId`, `heroId` e `questFlags` (array de strings).
É esse o formato que o migrador `SaveMigrator.FromV1` consome, e é o que está aqui.

O esboço em `06-arquitetura/save-schema.md` mostra um v1 mais antigo/solto (com `hero`,
`questFlags` como objeto, `abilities`, `collectibles`, `settings`). Se algum save real
de teste tiver sido gravado naquele formato, o importador precisa de um adaptador de
entrada antes do migrador. Mapear campo a campo, nunca por semelhança de texto.
