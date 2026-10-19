using System;
namespace SolDeCobre.Gameplay
{
    public enum AbilityPhase { Idle, Windup, Active, Recovery }

    // Máquina de estados pura de uma habilidade comprometida (ex.: AB-LACO).
    // Invariantes (abilities-e-combos.md):
    //  - Sem alvo: animação curta de windup e NENHUM gasto nem cooldown.
    //  - Soltar durante windup/ativo cancela em segurança, sem aplicar efeito/dano.
    //  - Não troca de alvo no meio da animação comprometida (o alvo é fixado no Begin).
    //  - Não pode iniciar durante Recovery (cooldown).
    // O tempo é injetado via Tick(dt): determinístico e testável sem engine.
    public sealed class AbilityController
    {
        readonly float windup, active, recovery;
        float timer;
        bool effectPending;

        public AbilityPhase Phase { get; private set; } = AbilityPhase.Idle;
        public bool EffectAppliedThisCycle { get; private set; }
        public bool IsReady => Phase == AbilityPhase.Idle;
        public bool OnCooldown => Phase == AbilityPhase.Recovery;

        public AbilityController(float windupSeconds, float activeSeconds, float recoverySeconds)
        {
            windup = Math.Max(0f, windupSeconds);
            active = Math.Max(0f, activeSeconds);
            recovery = Math.Max(0f, recoverySeconds);
        }

        // Inicia a habilidade. hasTarget vem da seleção; false = press sem alvo válido.
        public bool Begin(bool hasTarget)
        {
            if (Phase != AbilityPhase.Idle) return false;
            effectPending = hasTarget;
            EffectAppliedThisCycle = false;
            Enter(AbilityPhase.Windup);
            return true;
        }

        // Cancelamento seguro: nunca deixa efeito/dano ativo.
        public bool Cancel()
        {
            if (Phase != AbilityPhase.Windup && Phase != AbilityPhase.Active) return false;
            effectPending = false;
            Enter(AbilityPhase.Idle);
            return true;
        }

        public void Tick(float dt)
        {
            if (dt <= 0f || Phase == AbilityPhase.Idle) return;
            timer += dt;
            // Preserva o resto do tempo entre fases; 'while' resolve fases curtas
            // (duração ~0) num mesmo passo grande sem perder nem inventar tempo.
            bool advanced = true;
            while (advanced)
            {
                advanced = false;
                switch (Phase)
                {
                    case AbilityPhase.Windup when timer >= windup:
                        timer -= windup;
                        if (effectPending) { Phase = AbilityPhase.Active; advanced = true; }
                        else GoIdle();
                        break;
                    case AbilityPhase.Active when timer >= active:
                        timer -= active;
                        EffectAppliedThisCycle = true; // efeito só ao concluir o ativo
                        Phase = AbilityPhase.Recovery;
                        advanced = true;
                        break;
                    case AbilityPhase.Recovery when timer >= recovery:
                        timer -= recovery;
                        GoIdle();
                        break;
                }
            }
        }

        void Enter(AbilityPhase phase) { Phase = phase; timer = 0f; }
        void GoIdle() { Phase = AbilityPhase.Idle; timer = 0f; }
    }
}
