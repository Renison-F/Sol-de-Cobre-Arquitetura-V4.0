using UnityEngine;
using SolDeCobre.Core;
using SolDeCobre.Gameplay;
namespace SolDeCobre.World
{
    // BOSS-01 Fiscal de Trilhos. Dirige a máquina pura BossEncounter: tick por timer
    // (deadline; não espera callback de animação), laço na janela Exposed pontua,
    // Active abre a janela de dano ao herói, derrota publica BOSS_01_DEFEATED uma vez.
    // A física/arte do confronto e os blocos de carga são trabalho de cena.
    public sealed class FiscalBoss : MonoBehaviour, IThreatSource
    {
        [SerializeField] LassoAbility lasso;
        [SerializeField] string ringTargetId = "BOSS-01-RING"; // AbilityTargetSource do núcleo exposto
        [SerializeField] float introSeconds = 1f, exposedWindowSeconds = 1.5f, repositionSeconds = 1.5f;
        [SerializeField] bool assist;

        BossEncounter boss;
        GameSignals signals;
        bool started;

        public BossPhase Phase => boss?.Phase ?? BossPhase.Dormant;
        public bool DamageWindowOpen => boss != null && boss.DealsHeroDamageNow; // hitbox lê isto
        public float DamagePerHit => boss?.DamagePerHit ?? 1f;

        // IThreatSource: só fere na janela Active (sem contato quando inativo).
        public bool DamageActive => DamageWindowOpen;
        public float DamageAmount => DamagePerHit;
        public Transform Origin => transform;

        void Awake()
        {
            boss = new BossEncounter(introSeconds, exposedWindowSeconds, repositionSeconds);
            boss.SetAssist(assist);
        }
        // Liga no ACERTO efetivo (efeito aplicado), não no início do laço: assim
        // cancelar o laço durante windup/ativo não consome uma exposição.
        void OnEnable() { if (lasso != null) lasso.Applied += OnLassoApplied; }
        void OnDisable() { if (lasso != null) lasso.Applied -= OnLassoApplied; }

        // Injeta os sinais de domínio (publicação de flags). Chamado pelo bootstrap da cena.
        public void Bind(GameSignals gameSignals) => signals = gameSignals;

        public void Begin() { started = true; boss.Begin(); }
        public void SetPaused(bool paused) => boss.Freeze(paused);         // pausa congela timers
        public void RestoreCheckpoint() => boss.RestoreCurrentPhase();      // integridade da fase atual

        void Update()
        {
            if (started) boss.Tick(Time.deltaTime);
        }

        void OnLassoApplied(AbilityTargetSource source)
        {
            if (source == null || source.TargetId != ringTargetId) return; // só o núcleo do chefe
            var result = boss.TryLasso();
            if (result == LassoResult.Defeated && boss.TryPublishDefeat())
                signals?.OnQuestFlagGranted("BOSS_01_DEFEATED"); // uma única vez
            // ArmoredFeedback/Scored: apresentação (som/VFX) é tratada pela camada de view.
        }
    }
}
