using System;
using UnityEngine;
namespace SolDeCobre.Gameplay
{
    // AB-LACO (Mara): prende argola até 7 m usando a máquina pura AbilityController
    // e a seleção de alvo. Sem alvo: windup curto, sem gasto. Soltar interrompe em
    // segurança. A física do balanço/cabo é trabalho de cena (ver notas no fim).
    public sealed class LassoAbility : MonoBehaviour, IAbility
    {
        [SerializeField] AbilityTargeting targeting;
        [SerializeField] Transform aim; // sentido da mira (2.5D: +x/-x do herói)
        [SerializeField] float range = 7f, coneHalfAngle = 30f;
        [SerializeField] float windup = .12f, active = .25f, recovery = .3f;

        AbilityController controller;
        bool appliedFired;
        public string AbilityId => "AB-LACO";
        public AbilityTargetSource CurrentTarget { get; private set; }

        public event Action<AbilityTargetSource> Tethered; // apresentação: liga o cabo (no início)
        public event Action<AbilityTargetSource> Applied;  // ACERTO válido: efeito aplicado (fim do ativo)
        public event Action Released;                       // soltou/retornou ao neutro

        void Awake() => controller = new AbilityController(windup, active, recovery);

        public bool CanExecute() => controller != null && controller.IsReady;

        public bool TryExecute()
        {
            if (controller == null || !controller.IsReady) return false;
            Vector3 dir = aim != null ? aim.right : transform.right;
            AbilityTargetSource picked = null;
            bool hasTarget = targeting != null &&
                targeting.TryAcquire(transform.position, dir, range, coneHalfAngle, out picked);
            if (!controller.Begin(hasTarget)) return false;
            appliedFired = false;
            CurrentTarget = hasTarget ? picked : null;
            if (hasTarget) Tethered?.Invoke(picked);
            return true;
        }

        public void Interrupt()
        {
            if (controller == null) return;
            if (controller.Cancel()) ClearTether();
        }

        void Update()
        {
            if (controller == null) return;
            var before = controller.Phase;
            controller.Tick(Time.deltaTime);
            // Acerto válido só quando o efeito é aplicado (fim do ativo), não no windup:
            // cancelar antes disso não pontua (ver AbilityController.Cancel).
            if (!appliedFired && controller.EffectAppliedThisCycle)
            {
                appliedFired = true;
                Applied?.Invoke(CurrentTarget);
            }
            // Retorno ao neutro por fim natural do ciclo: solta o cabo e libera.
            if (before != AbilityPhase.Idle && controller.Phase == AbilityPhase.Idle)
                ClearTether();
        }

        void ClearTether()
        {
            CurrentTarget = null;
            appliedFired = false;
            Released?.Invoke();
        }
    }
}
