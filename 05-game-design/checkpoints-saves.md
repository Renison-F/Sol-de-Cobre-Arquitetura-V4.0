# Checkpoints, estados e restauração

Snapshot contém schemaVersion, checkpointId, chapterId, heroId, position, questFlags, abilities, collectibles e puzzleState. Não salvar por frame. Save é operação atômica com escrita em arquivo temporário e troca após validação de hash.

Resgate de corrupção: manter último checkpoint válido e opção de reiniciar capítulo. Save de tutorial e de capítulo não podem divergir quanto a desbloqueios.

**Casos:** queda/fechamento durante cutscene; morte no momento de troca; save pós-boss; migração de versão, múltiplas regiões/idiomas, dispositivo offline sem relógio confiável.
