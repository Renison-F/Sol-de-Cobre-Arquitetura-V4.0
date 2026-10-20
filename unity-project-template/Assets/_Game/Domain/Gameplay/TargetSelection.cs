using System;
using System.Collections.Generic;
namespace SolDeCobre.Gameplay
{
    // Candidato a alvo de habilidade, em termos puros (sem Transform/engine).
    // ConeAngle = ângulo absoluto em graus do centro da mira; Distance em metros.
    public readonly struct AbilityTarget
    {
        public readonly string Id;
        public readonly float Distance;
        public readonly float ConeAngle;
        public readonly bool IsMissionObject;
        public AbilityTarget(string id, float distance, float coneAngle, bool isMissionObject)
        {
            Id = id; Distance = distance; ConeAngle = coneAngle; IsMissionObject = isMissionObject;
        }
    }

    // Prioridade (abilities-e-combos.md): objeto de missão válido no cone →
    // alvo central mais próximo → desempate por ID estável. Alvo fora de alcance
    // ou fora do cone não é selecionável (não trava: apenas não há alvo).
    public static class TargetSelection
    {
        public static bool TrySelect(IReadOnlyList<AbilityTarget> candidates,
            float range, float coneHalfAngle, out AbilityTarget chosen)
        {
            chosen = default;
            if (candidates == null) return false;
            bool found = false;
            for (int i = 0; i < candidates.Count; i++)
            {
                var c = candidates[i];
                if (string.IsNullOrEmpty(c.Id)) continue;
                if (c.Distance < 0f || c.Distance > range) continue;
                if (c.ConeAngle < 0f || c.ConeAngle > coneHalfAngle) continue;
                if (!found || Better(c, chosen)) { chosen = c; found = true; }
            }
            return found;
        }

        // a é melhor que b? Missão antes; depois mais central; depois mais próximo;
        // desempate final por ID ordinal estável.
        static bool Better(AbilityTarget a, AbilityTarget b)
        {
            if (a.IsMissionObject != b.IsMissionObject) return a.IsMissionObject;
            if (!Near(a.ConeAngle, b.ConeAngle)) return a.ConeAngle < b.ConeAngle;
            if (!Near(a.Distance, b.Distance)) return a.Distance < b.Distance;
            return string.CompareOrdinal(a.Id, b.Id) < 0;
        }

        static bool Near(float x, float y) => Math.Abs(x - y) <= 1e-4f;
    }
}
