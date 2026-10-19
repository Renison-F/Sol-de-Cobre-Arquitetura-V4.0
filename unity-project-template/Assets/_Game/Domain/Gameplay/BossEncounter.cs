using System;
namespace SolDeCobre.Gameplay
{
    public enum BossPhase { Dormant, Intro, Telegraph, Active, Recover, Exposed, Reposition, Defeated }
    public enum BossAttack { Carimbo, Varredura }
    public enum LassoResult { ArmoredFeedback, Scored, Defeated, AlreadyDefeated }

    // Encontro do Fiscal de Trilhos (BOSS-01). 3 exposições = contagem de integridade;
    // um acerto válido por exposição (laço na janela Exposed). Fora da janela o chefe
    // está blindado: laço dá feedback sem dano. Todo ataque avança por timer (deadline;
    // nunca espera callback de animação). Pausa congela timers. Checkpoint restaura a
    // fase atual. Derrota ocorre e é publicada uma única vez (vitória idempotente).
    public sealed class BossEncounter
    {
        readonly struct Spec
        {
            public readonly float Telegraph, Active, Recover;
            public Spec(float t, float a, float r) { Telegraph = t; Active = a; Recover = r; }
        }
        // Tabela de BOSS-01.md.
        static readonly Spec Carimbo = new Spec(1.2f, 0.3f, 2.5f);
        static readonly Spec Varredura = new Spec(1.4f, 0.6f, 1.8f);
        const float AssistWarningScale = 1.35f;
        const int ExposuresNeeded = 3;

        readonly float intro, exposedWindow, repositionPause;
        float timer;
        int exposuresDone;
        bool frozen, assist, defeatPublished;
        BossAttack current = BossAttack.Carimbo, last = BossAttack.Varredura;

        public BossPhase Phase { get; private set; } = BossPhase.Dormant;
        public BossAttack CurrentAttack => current;
        public int ExposuresDone => exposuresDone;
        public int ExposuresRemaining => ExposuresNeeded - exposuresDone;
        // Fase lógica 1..3 (BOSS-01.md): nº de exposições já concluídas define a fase.
        public int DifficultyPhase => exposuresDone < 1 ? 1 : exposuresDone < 2 ? 2 : 3;
        // Sem colisão de contato enquanto o chefe está inativo: só fere na janela Active.
        public bool DealsHeroDamageNow => Phase == BossPhase.Active;
        public float DamagePerHit => assist ? 0.5f : 1f;

        public BossEncounter(float introSeconds = 1f, float exposedWindowSeconds = 1.5f, float repositionSeconds = 1.5f)
        {
            intro = Math.Max(0f, introSeconds);
            exposedWindow = Math.Max(0.01f, exposedWindowSeconds);
            repositionPause = Math.Max(0f, repositionSeconds);
        }

        public void SetAssist(bool value) => assist = value;
        public void Freeze(bool value) => frozen = value;

        public void Begin()
        {
            if (Phase != BossPhase.Dormant) return;
            Phase = BossPhase.Intro; timer = 0f;
        }

        public void Tick(float dt)
        {
            if (frozen || dt <= 0f) return;
            if (Phase == BossPhase.Dormant || Phase == BossPhase.Defeated) return;
            timer += dt; // uma vez por tick; transições zeram o timer ao entrar na próxima fase
            switch (Phase)
            {
                case BossPhase.Intro: if (timer >= intro) EnterCycle(); break;
                case BossPhase.Telegraph: if (timer >= Warning(current)) { Phase = BossPhase.Active; timer = 0f; } break;
                case BossPhase.Active: if (timer >= SpecOf(current).Active) { Phase = BossPhase.Recover; timer = 0f; } break;
                case BossPhase.Recover: if (timer >= SpecOf(current).Recover) { Phase = BossPhase.Exposed; timer = 0f; } break;
                case BossPhase.Exposed: if (timer >= exposedWindow) EnterCycle(); break; // janela perdida: próximo ciclo
                case BossPhase.Reposition: if (timer >= repositionPause) { Phase = BossPhase.Telegraph; timer = 0f; } break;
            }
        }

        // Laço do herói. Só pontua na janela Exposed; blindado fora dela.
        public LassoResult TryLasso()
        {
            if (Phase == BossPhase.Defeated) return LassoResult.AlreadyDefeated;
            if (Phase != BossPhase.Exposed) return LassoResult.ArmoredFeedback;
            exposuresDone++;
            timer = 0f;
            if (exposuresDone >= ExposuresNeeded)
            {
                Phase = BossPhase.Defeated; // uma única vez; cancela hitboxes
                return LassoResult.Defeated;
            }
            EnterCycle(); // PhaseTransition cancela hitboxes/projéteis antigos
            return LassoResult.Scored;
        }

        // Publica BOSS_01_DEFEATED uma única vez (recompensa idempotente é do MissionProgress).
        public bool TryPublishDefeat()
        {
            if (Phase != BossPhase.Defeated || defeatPublished) return false;
            defeatPublished = true; return true;
        }

        // Volta ao checkpoint: restaura a fase atual (mantém exposições já concluídas),
        // cancela o ataque em andamento e recomeça o ciclo da fase.
        public void RestoreCurrentPhase()
        {
            if (Phase == BossPhase.Defeated) return;
            EnterCycle();
        }

        void EnterCycle()
        {
            ChooseAttack();
            // Fase 3: reposiciona com pausa antes do próximo telegraph.
            Phase = DifficultyPhase >= 3 ? BossPhase.Reposition : BossPhase.Telegraph;
            timer = 0f;
        }

        void ChooseAttack()
        {
            // Fase 1: só Carimbo. Fase 2+: alterna sem sobrepor intervalos ativos.
            current = DifficultyPhase <= 1 ? BossAttack.Carimbo
                    : (last == BossAttack.Carimbo ? BossAttack.Varredura : BossAttack.Carimbo);
            last = current;
        }

        float Warning(BossAttack a) => SpecOf(a).Telegraph * (assist ? AssistWarningScale : 1f);
        static Spec SpecOf(BossAttack a) => a == BossAttack.Carimbo ? Carimbo : Varredura;
    }
}
