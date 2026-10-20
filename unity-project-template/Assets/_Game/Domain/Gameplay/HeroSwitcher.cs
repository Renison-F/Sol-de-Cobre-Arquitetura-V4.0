namespace SolDeCobre.Gameplay
{
    public enum SwitchResult { Switched, Queued, Rejected, Dropped }

    // Troca segura de herói (abilities-e-combos.md / modulos-e-contratos.md):
    // indisponível no ar, em hit reaction, interação crítica ou resgate; o pedido pode
    // ser enfileirado por 0,3 s e executado no próximo solo seguro. Puro e testável;
    // "grounded" e "blocked" vêm do runtime. Snapshot (SessionState) é preservado à parte.
    public sealed class HeroSwitcher
    {
        const float QueueWindow = 0.3f;
        float queueTimer;

        public string CurrentHero { get; private set; }
        public bool Unlocked { get; set; } // segundo herói liberado (ZE_JOINED, cap. 2+)
        public bool HasQueued => queueTimer > 0f;

        public HeroSwitcher(string startHero = "mara", bool unlocked = false)
        {
            CurrentHero = startHero == "ze" ? "ze" : "mara";
            Unlocked = unlocked;
        }

        // blocked = no ar não; blocked cobre hit reaction / interação crítica / resgate.
        public SwitchResult Request(bool grounded, bool blocked)
        {
            if (!Unlocked) return SwitchResult.Rejected;      // só um herói disponível
            if (blocked) return SwitchResult.Rejected;        // estado incompatível: sem fila
            if (!grounded) { queueTimer = QueueWindow; return SwitchResult.Queued; } // no ar: enfileira
            Toggle();
            return SwitchResult.Switched;
        }

        // Avança a fila; executa no próximo solo seguro dentro da janela, senão descarta.
        public SwitchResult Tick(float dt, bool grounded, bool blocked)
        {
            if (queueTimer <= 0f) return SwitchResult.Dropped; // nada enfileirado
            if (blocked) { queueTimer = 0f; return SwitchResult.Dropped; }
            if (grounded) { queueTimer = 0f; Toggle(); return SwitchResult.Switched; }
            queueTimer -= dt;
            if (queueTimer <= 0f) { queueTimer = 0f; return SwitchResult.Dropped; }
            return SwitchResult.Queued;
        }

        void Toggle() => CurrentHero = CurrentHero == "mara" ? "ze" : "mara";
    }
}
