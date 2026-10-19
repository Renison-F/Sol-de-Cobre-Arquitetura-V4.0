using System;
namespace SolDeCobre.Core
{
    // Eventos de domínio: a camada de UI jamais deve ser a fonte da verdade.
    public sealed class GameSignals
    {
        public event Action<string> CheckpointReached;
        public event Action<string> LevelCompleted;
        public event Action<string> QuestFlagGranted;
        public void OnCheckpointReached(string checkpointId) => CheckpointReached?.Invoke(checkpointId);
        public void OnLevelCompleted(string levelId) => LevelCompleted?.Invoke(levelId);
        public void OnQuestFlagGranted(string flagId) => QuestFlagGranted?.Invoke(flagId);
    }
}
