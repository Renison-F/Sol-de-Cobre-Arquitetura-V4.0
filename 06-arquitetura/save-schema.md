# Schema de save e migrações

```json
{"schemaVersion":1,"slotId":"local-01","chapterId":"LVL-03","checkpointId":"LVL-03-CP-02","hero":"mara","questFlags":{"M01":true,"M02":true},"collectibles":[],"abilities":["lasso"],"settings":{"reduceFlash":false}}
```

Chaves e IDs estáveis; serializar números com formato invariável; não armazenar localização absoluta entre revisões de nível sem id de checkpoint. Escrever snapshot em temp + substituição; manter backup; migrações testadas por fixture por versão.
