using UnityEngine;
namespace SolDeCobre.Gameplay
{
    [CreateAssetMenu(menuName="Sol de Cobre/Hero Definition")]
    public sealed class HeroDefinition : ScriptableObject
    {
        public string heroId;
        public float moveSpeed = 5f;
        public float jumpHeight = 2f;
        public float gravity = -18f;
        public bool canUseLasso;
        public bool canUseElectricCharge;
    }
}
