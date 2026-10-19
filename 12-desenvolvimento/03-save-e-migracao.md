# Save, slots e compatibilidade

Formato alvo `save.schema.json` versão 3. Campos obrigatórios e IDs estáveis são definidos no contrato. Converter explicitamente V2 `schemaVersion=1`, `chapterId`, `checkpointId`, `heroId`, `questFlags` para novos nomes e valores; valor desconhecido produz diagnóstico recuperável. Não mapear flag por semelhança de texto.

Migração 1→3 cria cópia e `revision=1`, preserva capítulo, herói e flags conhecidas; opções recebem defaults documentados. Checkpoint inválido volta à entrada segura do mesmo capítulo, com aviso. Fragmentos ausentes recebem conjunto vazio; itens antigos desconhecidos são guardados em relatório externo de migração para investigação, não convertidos em compras ou habilidades.

Três slots independentes; opções globais podem ficar em arquivo separado versionado. Antes de salvar, validar schema e catálogos; gravar em temporário, flush e promoção atômica com backup. Save só publica sucesso depois de persistência confirmada; HUD indica falha sem spammar frames. Morte e troca de herói não criam nova revisão a cada tick.

Os scripts C# incluídos continuam exemplo de schema 1, com proteção de I/O ampliada, e não implementam a migração completa alvo. Implementar novo DTO, migrador e testes é DEV-08/DEV-09. Isso impede que o desenvolvedor confunda um exemplo compatível com o modelo final.

Ensaio: corrupção de byte, truncamento, disco cheio, permissão negada, interrupção entre tmp e promoção, versão futura, dois saves concorrentes, backup corrompido, fechamento durante checkpoint e importação acima de 1 MiB. Não registrar conteúdo do save em telemetria.
