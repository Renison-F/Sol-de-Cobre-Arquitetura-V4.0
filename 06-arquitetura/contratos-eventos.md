# Eventos de domínio e idempotência

Eventos: `AbilityExecuted`, `CheckpointReached`, `QuestStageAdvanced`, `CutsceneCompleted`, `HeroSwitched`, `WaterChannelOpened`, `LevelCompleted`.

Envelope: `eventId`, `eventType`, `aggregateId`, `chapterId`, `payload`, `timestampTick`. `QuestStageAdvanced` ignora eventos repetidos; HUD e VFX são assinantes sem autoridade sobre estado. Skipping cutscene dispara conclusão exatamente uma vez.
