using System;
using System.Collections.Generic;
using UnityEngine;
using SolDeCobre.Core;
namespace SolDeCobre.Narrative
{
    // Liga a missão ordenada (M01) aos sinais de domínio: a fonte de progresso são
    // eventos (OnQuestFlagGranted), nunca clique de UI. Recompensa concedida uma vez.
    // A HUD mostra NextObjective (próxima ação concreta), sem exigir leitura de IDs.
    public sealed class MissionController : MonoBehaviour
    {
        [SerializeField] string[] orderedSteps =
        {
            "POCO_EXAMINADO", "LACO_APRENDIDO", "DESVIO_ENCONTRADO",
            "PATIO_ABERTO", "BOSS_01_DEFEATED", "LVL_01_COMPLETE"
        };

        MissionProgress progress;
        GameSignals signals;

        public event Action Completed;   // dispara uma única vez, ao conceder a recompensa
        public string NextObjective => progress?.NextExpected;
        public bool IsComplete => progress != null && progress.Completed;

        void Awake() => progress = new MissionProgress(orderedSteps);

        public void Bind(GameSignals gameSignals)
        {
            if (signals != null) signals.QuestFlagGranted -= OnFlag;
            signals = gameSignals;
            if (signals != null) signals.QuestFlagGranted += OnFlag;
        }
        void OnDestroy() { if (signals != null) signals.QuestFlagGranted -= OnFlag; }

        public void Restore(ICollection<string> completedFlags, bool rewardGranted) =>
            progress.Restore(completedFlags, rewardGranted);

        void OnFlag(string flag)
        {
            if (progress.Report(flag) == MissionEventResult.Completed && progress.TryGrantReward())
                Completed?.Invoke(); // liberar capítulo 2 / atlas / M01_COMPLETE na camada de jogo
        }
    }
}
