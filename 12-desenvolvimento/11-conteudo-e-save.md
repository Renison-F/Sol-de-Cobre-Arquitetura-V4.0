# Conteúdo, IDs e migração de save sem retrabalho

## Identificadores e pipeline

11-contratos contém os catálogos autorais de campanha, elenco, inimigos e backlog. modelagem.json acrescenta metas específicas de 3D; modelagem-resumo.csv é uma visão derivada e deve acompanhar os valores do JSON. Não manter outra planilha com budgets divergentes. Importação Unity deve produzir definições e validar IDs antes de gerar cenas completas. A automação atual gera apenas o laboratório, sem importar a campanha.

CHR-MARA-VENTO é identidade narrativa/asset; mara é alias do herói no save; mara-vento é slug de documento. Mapeamentos devem ser explícitos. LVL-01 é capítulo; POCO_EXAMINADO é flag. Nome visível pode mudar sem invalidar progresso. A troca para Isadora Veredas ocorreu antes da produção; depois do primeiro save público, renomear IDs exige aliases e migração versionada.

## Contrato de migração

Schema 1 legado tem chapterId/checkpointId/heroId/questFlags; schema3 alvo tem schema_version/slot/revision/chapter/checkpoint/hero/flags/atlas/written_at/options. Implementar migrador puro, catálogo permitido de flags e resultado Migrated/Unsupported/Corrupt. Mapear campo a campo e preservar entrada original. Nunca sobrescrever save de versão futura nem inferir propriedade de compra a partir de flags locais.

Migração não executa comandos de gameplay para cada flag; aplica snapshot consolidado. Checkpoint inexistente usa entrada segura do mesmo capítulo com diagnóstico. Adicionar opção recebe default documentado, não null silencioso. Alteração de estrutura de nível precisa de mapeamento explícito de checkpoints antigos. Antes do primeiro release, exportar fixtures de saves por versão para regressão.

## Casos indispensáveis

1. Schema1 válido com capítulo/herói/flags conhecidos → schema3 equivalente, nova cópia e revisão, original preservado.
2. JSON truncado, versão futura ou flag desconhecida → resultado de recuperação, sem reset silencioso.
3. Disco cheio/sem permissão e interrupção antes da promoção → save anterior válido e HUD sem falso sucesso.
4. Duas solicitações de save → fila/serialização com revisão crescente, sem gravar estado velho por último.
5. Suspensão em ataque/cutscene/troca de herói → restauração em estado seguro, sem hitbox ativa.

O repositório C# de exemplo continua legado e precisa de adaptador atômico validado por plataforma. O runner/laboratório não exercitam esses casos. DEV-08/09/11 e QA-02 continuam responsáveis por essa entrega produtiva.
