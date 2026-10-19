using System;
namespace SolDeCobre.Gameplay
{
    public enum CompanionPhase { Home, Outbound, Returning }

    // Despacho do mascote Faísca (AB-TUNEL): vai ao canal (ação ~1,5 s), ativa o
    // objeto e retorna ao dono (~2 s). Sem rota válida, pede dica e não despacha.
    // Invariante central (abilities-e-combos.md): o mascote SEMPRE retorna ao dono —
    // inclusive se cancelado no meio (então volta sem ativar).
    public sealed class CompanionDispatch
    {
        readonly float outbound, back;
        float timer;

        public CompanionPhase Phase { get; private set; } = CompanionPhase.Home;
        public bool ActivatedThisTrip { get; private set; }
        public bool RouteHintRequested { get; private set; }

        public CompanionDispatch(float outboundSeconds = 1.5f, float returnSeconds = 2f)
        {
            outbound = Math.Max(0.0001f, outboundSeconds);
            back = Math.Max(0.0001f, returnSeconds);
        }

        public bool Dispatch(bool hasRoute)
        {
            if (Phase != CompanionPhase.Home) return false;
            if (!hasRoute) { RouteHintRequested = true; return false; }
            RouteHintRequested = false;
            ActivatedThisTrip = false;
            timer = 0f;
            Phase = CompanionPhase.Outbound;
            return true;
        }

        // Cancelar/recolher: de Outbound volta sem ativar, a partir da posição atual
        // (sem salto visual); de Returning já está voltando.
        public bool Recall()
        {
            if (Phase != CompanionPhase.Outbound) return false;
            float awayFraction = Math.Clamp(timer / outbound, 0f, 1f);
            timer = back * (1f - awayFraction); // continuidade: 1 - timer/back == awayFraction
            Phase = CompanionPhase.Returning;
            return true;
        }

        public void Tick(float dt)
        {
            if (dt <= 0f) return;
            timer += dt;
            if (Phase == CompanionPhase.Outbound && timer >= outbound)
            {
                timer -= outbound;
                ActivatedThisTrip = true; // chegou ao canal e ativou o objeto
                Phase = CompanionPhase.Returning;
            }
            if (Phase == CompanionPhase.Returning && timer >= back)
            {
                timer = 0f;
                Phase = CompanionPhase.Home; // retornou ao dono
            }
        }

        // Fração de afastamento do dono (0 em casa, 1 no alvo) — para o runtime posicionar.
        public float AwayFraction()
        {
            switch (Phase)
            {
                case CompanionPhase.Outbound: return Math.Clamp(timer / outbound, 0f, 1f);
                case CompanionPhase.Returning: return Math.Clamp(1f - timer / back, 0f, 1f);
                default: return 0f;
            }
        }
    }
}
