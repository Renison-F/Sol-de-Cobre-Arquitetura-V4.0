using System.Collections.Generic;
using UnityEngine;
namespace SolDeCobre.Gameplay
{
    // Marca um objeto da cena como alvo de habilidade (argola, canal, interruptor).
    // Mantém um registro ativo para a mira consultar sem varrer a cena inteira.
    public sealed class AbilityTargetSource : MonoBehaviour
    {
        [SerializeField] string targetId;
        [SerializeField] bool missionObject;

        public string TargetId => targetId;
        public bool IsMission => missionObject;
        public Vector3 Position => transform.position;

        static readonly List<AbilityTargetSource> active = new List<AbilityTargetSource>();
        public static IReadOnlyList<AbilityTargetSource> Active => active;

        void OnEnable()
        {
            if (string.IsNullOrEmpty(targetId)) { Debug.LogWarning("AbilityTargetSource sem id.", this); return; }
            if (!active.Contains(this)) active.Add(this);
        }
        void OnDisable() => active.Remove(this);
    }
}
