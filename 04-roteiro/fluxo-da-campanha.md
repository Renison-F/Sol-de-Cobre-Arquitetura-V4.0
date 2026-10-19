# Fluxo macro e dependências de campanha

```mermaid
flowchart TD
  A[LVL-01 Estação] --> B[LVL-02 Cânion]
  B --> C[LVL-03 Minas]
  C --> D[LVL-04 Cidade Fantasma]
  D --> E[LVL-05 Expresso]
  E --> F[LVL-06 Oásis]
  F --> G[LVL-07 Serra]
  G --> H[LVL-08 Observatório]
  H --> I[LVL-09 Cidadela]
  I --> J[LVL-10 Coração]
```

**Nota:** o diagrama textual acima é especificação, não requer dependência de CDN. O portal contém um fluxograma próprio, interativo e offline.

## Dependências persistentes
`knows_mara_mother=true` após LVL-08; `ze_confessed=true` após LVL-07; `water_channels_safe` exige os quatro canais no LVL-09; `finale_restorative` requer segredos do Oásis e Observatório. Nenhuma flag opcional bloqueia o final padrão.
