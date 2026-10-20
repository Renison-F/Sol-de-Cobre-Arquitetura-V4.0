using System.Collections.Generic;
using UnityEngine;
namespace SolDeCobre.Gameplay
{
    // Ponte entre a cena e a seleção pura: monta candidatos (distância + ângulo do
    // cone) a partir das AbilityTargetSource ativas e delega a TargetSelection.
    public sealed class AbilityTargeting : MonoBehaviour
    {
        readonly List<AbilityTarget> buffer = new List<AbilityTarget>();
        readonly Dictionary<string, AbilityTargetSource> byId = new Dictionary<string, AbilityTargetSource>();

        public bool TryAcquire(Vector3 origin, Vector3 aimDir, float range, float coneHalfAngle, out AbilityTargetSource source)
        {
            source = null;
            buffer.Clear(); byId.Clear();
            aimDir = aimDir.sqrMagnitude < 1e-6f ? Vector3.right : aimDir.normalized;

            var sources = AbilityTargetSource.Active;
            for (int i = 0; i < sources.Count; i++)
            {
                var s = sources[i];
                if (s == null || string.IsNullOrEmpty(s.TargetId)) continue;
                Vector3 to = s.Position - origin;
                float dist = to.magnitude;
                if (dist > range) continue;
                float angle = dist < 1e-4f ? 0f : Vector3.Angle(aimDir, to);
                byId[s.TargetId] = s; // ids devem ser únicos entre fontes ativas
                buffer.Add(new AbilityTarget(s.TargetId, dist, angle, s.IsMission));
            }

            if (TargetSelection.TrySelect(buffer, range, coneHalfAngle, out var chosen) &&
                byId.TryGetValue(chosen.Id, out source))
                return true;
            source = null;
            return false;
        }
    }
}
