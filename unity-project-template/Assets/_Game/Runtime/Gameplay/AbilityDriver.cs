using UnityEngine;
namespace SolDeCobre.Gameplay
{
    // Liga a ponte de input à habilidade do herói ativo. Press executa; soltar
    // interrompe em segurança. Troca de herói reatribui a habilidade corrente.
    public sealed class AbilityDriver : MonoBehaviour
    {
        [SerializeField] PlayerInputBridge input;
        [SerializeField] MonoBehaviour abilitySource; // deve implementar IAbility
        IAbility ability;

        void Awake()
        {
            ability = abilitySource as IAbility;
            if (ability == null)
                Debug.LogError("AbilityDriver precisa de um MonoBehaviour que implemente IAbility.", this);
        }
        void OnEnable()
        {
            if (input == null) return;
            input.AbilityPressed += OnPressed;
            input.AbilityReleased += OnReleased;
        }
        void OnDisable()
        {
            if (input == null) return;
            input.AbilityPressed -= OnPressed;
            input.AbilityReleased -= OnReleased;
        }

        public void SetAbility(IAbility next) => ability = next;

        void OnPressed() => ability?.TryExecute();
        void OnReleased() => ability?.Interrupt();
    }
}
