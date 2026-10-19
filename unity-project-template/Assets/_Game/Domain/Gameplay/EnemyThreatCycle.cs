using System;
namespace SolDeCobre.Gameplay
{
    public enum EnemyPhase { Dormant, Idle, Prepare, Active, Recover, Defeated }

    // Lógica compartilhada das famílias de inimigo (patrol/hover/dive/turret).
    // Ameaça só tem Prepare/Active/Recover; Dormant/Defeated são ciclo de vida.
    // Dano só na janela Active; recuo mínimo entre ações; pausa/carregamento não
    // disparam ataque imediatamente; derrota publica ID uma única vez.
    // Movimento e linha de visão ficam no runtime; aqui só o relógio de ameaça.
    public sealed class EnemyThreatCycle
    {
        const float AssistWarningScale = 1.35f;
        readonly float warning, active, recover, minGap;
        int integrity;
        float timer, gapRemaining;
        bool frozen, aggro, assist;

        public EnemyPhase Phase { get; private set; } = EnemyPhase.Dormant;
        public bool DealsDamageNow => Phase == EnemyPhase.Active;
        public bool IdPublished { get; private set; }

        public EnemyThreatCycle(int integrity, float warningSeconds, float activeSeconds,
            float recoverSeconds, float minGapSeconds = 1.5f, bool assist = false)
        {
            this.integrity = Math.Max(1, integrity);
            warning = Math.Max(0f, warningSeconds);
            active = Math.Max(0f, activeSeconds);
            recover = Math.Max(0f, recoverSeconds);
            minGap = Math.Max(0f, minGapSeconds);
            this.assist = assist;
        }

        public int Integrity => integrity;
        public void SetAssist(bool value) => assist = value;
        public void Freeze(bool value) => frozen = value;
        public void SetAggro(bool value) => aggro = value; // exige LOS + raio no runtime

        // Entra no mundo em estado neutro; não ataca no mesmo instante (gap inicial).
        public void Activate()
        {
            if (Phase != EnemyPhase.Dormant) return;
            Phase = EnemyPhase.Idle; timer = 0f; gapRemaining = minGap;
        }

        // Morte do herói: volta ao neutro e cancela hitbox; integridade preservada.
        public void ResetNeutral()
        {
            if (Phase == EnemyPhase.Defeated) return;
            Phase = EnemyPhase.Idle; timer = 0f; gapRemaining = minGap;
        }

        // Resposta válida (laço/impacto no ponto fraco). Interrompe a ameaça atual.
        public bool TakeValidHit()
        {
            if (Phase == EnemyPhase.Defeated || Phase == EnemyPhase.Dormant) return false;
            integrity--;
            if (integrity <= 0)
            {
                integrity = 0; Phase = EnemyPhase.Defeated; timer = 0f;
                return true; // derrotado agora
            }
            Phase = EnemyPhase.Idle; timer = 0f; gapRemaining = minGap; // stagger + cancela hitbox
            return false;
        }

        // Publica o ID de derrota uma única vez (devolve ao pool depois do efeito).
        public bool TryPublishDefeat()
        {
            if (Phase != EnemyPhase.Defeated || IdPublished) return false;
            IdPublished = true; return true;
        }

        public void Tick(float dt)
        {
            if (frozen || dt <= 0f) return;
            switch (Phase)
            {
                case EnemyPhase.Idle:
                    if (gapRemaining > 0f) gapRemaining = Math.Max(0f, gapRemaining - dt);
                    if (aggro && gapRemaining <= 0f) { Phase = EnemyPhase.Prepare; timer = 0f; }
                    break;
                case EnemyPhase.Prepare:
                    timer += dt;
                    if (timer >= warning * (assist ? AssistWarningScale : 1f)) { Phase = EnemyPhase.Active; timer = 0f; }
                    break;
                case EnemyPhase.Active:
                    timer += dt;
                    if (timer >= active) { Phase = EnemyPhase.Recover; timer = 0f; }
                    break;
                case EnemyPhase.Recover:
                    timer += dt;
                    if (timer >= recover) { Phase = EnemyPhase.Idle; timer = 0f; gapRemaining = minGap; }
                    break;
            }
        }
    }
}
