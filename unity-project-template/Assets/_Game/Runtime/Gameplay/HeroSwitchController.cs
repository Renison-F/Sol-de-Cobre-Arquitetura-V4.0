using System;
using UnityEngine;
namespace SolDeCobre.Gameplay
{
    // Liga o input de troca de herói à regra pura HeroSwitcher: bloqueada no ar/hit/
    // crítico/resgate e enfileirável por 0,3 s. A troca de MODELO/mascote (arte) é backlog;
    // aqui garante a regra segura e expõe o herói lógico atual + evento HeroChanged.
    public sealed class HeroSwitchController : MonoBehaviour
    {
        [SerializeField] PlayerInputBridge input;
        [SerializeField] CharacterController character; // para saber se está no chão
        [SerializeField] bool secondHeroUnlocked;       // ZE_JOINED (cap. 2+)

        HeroSwitcher switcher;

        // Setado por hit reaction / interação crítica / resgate (estado incompatível).
        public bool Blocked { get; set; }
        public string CurrentHero => switcher != null ? switcher.CurrentHero : "mara";
        public event Action<string> HeroChanged;

        void Awake() => switcher = new HeroSwitcher("mara", secondHeroUnlocked);
        public void SetUnlocked(bool value) { if (switcher != null) switcher.Unlocked = value; }

        void OnEnable() { if (input != null) input.HeroSwitchRequested += OnRequest; }
        void OnDisable() { if (input != null) input.HeroSwitchRequested -= OnRequest; }

        bool Grounded => character == null || character.isGrounded;

        void OnRequest()
        {
            if (switcher.Request(Grounded, Blocked) == SwitchResult.Switched)
                HeroChanged?.Invoke(switcher.CurrentHero);
        }

        void Update()
        {
            if (switcher == null || !switcher.HasQueued) return;
            if (switcher.Tick(Time.deltaTime, Grounded, Blocked) == SwitchResult.Switched)
                HeroChanged?.Invoke(switcher.CurrentHero);
        }
    }
}
