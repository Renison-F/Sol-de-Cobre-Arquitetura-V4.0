using System.Collections.Generic;
namespace SolDeCobre.Narrative
{
    public enum MissionEventResult { Advanced, Completed, Duplicate, Rejected }

    // Progresso de missão por ETAPAS ORDENADAS (M01). Fonte de progresso são eventos
    // de domínio, nunca clique de UI. Estado fora de ordem é rejeitado; evento repetido
    // ou atrasado é no-op seguro; recompensa é concedida uma única vez.
    public sealed class MissionProgress
    {
        readonly string[] steps;
        int next;               // índice da próxima etapa esperada
        public bool Completed => next >= steps.Length;
        public bool RewardGranted { get; private set; }
        // Próxima ação concreta para a HUD (sem exigir leitura de IDs); null se completo.
        public string NextExpected => Completed ? null : steps[next];
        public int CompletedCount => next;

        public MissionProgress(IReadOnlyList<string> orderedSteps)
        {
            var list = new List<string>();
            if (orderedSteps != null)
                foreach (var s in orderedSteps)
                    if (!string.IsNullOrEmpty(s)) list.Add(s);
            steps = list.ToArray();
        }

        public MissionEventResult Report(string flag)
        {
            if (string.IsNullOrEmpty(flag)) return MissionEventResult.Rejected;
            if (Completed) return MissionEventResult.Duplicate; // evento após o fim: no-op seguro
            if (flag == steps[next])
            {
                next++;
                return Completed ? MissionEventResult.Completed : MissionEventResult.Advanced;
            }
            // Já consumida (evento atrasado/duplicado) → no-op; qualquer outra → fora de ordem.
            for (int i = 0; i < next; i++) if (steps[i] == flag) return MissionEventResult.Duplicate;
            return MissionEventResult.Rejected;
        }

        // Recompensa idempotente por missão: só concede quando completa e ainda não concedida.
        public bool TryGrantReward()
        {
            if (!Completed || RewardGranted) return false;
            RewardGranted = true;
            return true;
        }

        // Restauração por save/checkpoint: avança pelo maior prefixo em ordem presente no conjunto.
        public void Restore(ICollection<string> completedFlags, bool rewardGranted)
        {
            next = 0;
            if (completedFlags != null)
                while (next < steps.Length && completedFlags.Contains(steps[next])) next++;
            RewardGranted = rewardGranted && Completed;
        }
    }
}
