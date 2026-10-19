using UnityEngine;
using SolDeCobre.Gameplay;
namespace SolDeCobre.World
{
    // Mascote Faísca (AB-TUNEL): no contexto de interação, se há canal em alcance,
    // despacha; percorre e ativa o objeto; SEMPRE retorna ao dono (CompanionDispatch).
    // Sem rota válida, pede dica. A posição é interpolada entre dono e canal.
    public sealed class CompanionController : MonoBehaviour
    {
        [SerializeField] Transform owner, companion;
        [SerializeField] PlayerInputBridge input;
        [SerializeField] AbilityTargeting targeting;
        [SerializeField] float range = 6f, coneHalfAngle = 40f, outbound = 1.5f, back = 2f;

        CompanionDispatch dispatch;
        AbilityTargetSource channel;

        public bool HintRequested { get; private set; }

        void Awake() => dispatch = new CompanionDispatch(outbound, back);
        void OnEnable() { if (input != null) input.Interacted += OnInteract; }
        void OnDisable() { if (input != null) input.Interacted -= OnInteract; }

        void OnInteract()
        {
            HintRequested = false;
            Vector3 origin = owner != null ? owner.position : transform.position;
            Vector3 dir = owner != null ? owner.right : transform.right;
            bool hasRoute = targeting != null &&
                targeting.TryAcquire(origin, dir, range, coneHalfAngle, out channel);
            if (!dispatch.Dispatch(hasRoute))
                HintRequested = dispatch.RouteHintRequested; // sem rota: sinaliza dica
        }

        // Recolher em segurança (ex.: contexto perdido/troca de herói): volta ao dono.
        public void Recall() => dispatch.Recall();

        void Update()
        {
            if (dispatch == null) return;
            dispatch.Tick(Time.deltaTime);
            if (companion == null || owner == null) return;
            Vector3 home = owner.position;
            Vector3 away = channel != null ? channel.Position : home;
            companion.position = Vector3.Lerp(home, away, dispatch.AwayFraction());
        }
    }
}
