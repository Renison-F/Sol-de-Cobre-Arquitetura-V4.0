using UnityEngine;
namespace SolDeCobre.Gameplay
{
    [CreateAssetMenu(menuName="Sol de Cobre/Hero Definition")]
    public sealed class HeroDefinition : ScriptableObject
    {
        public string heroId;
        // Baseline "responsivo": gravidade forte evita flutuar; ajuste fino no Inspector.
        public float moveSpeed = 7f;
        public float jumpHeight = 2.3f;
        public float gravity = -32f;
        public bool canUseLasso;
        public bool canUseElectricCharge;
    }
}
