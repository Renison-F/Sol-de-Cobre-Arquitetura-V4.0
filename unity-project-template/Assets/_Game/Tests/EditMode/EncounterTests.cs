using System.Collections.Generic;
using NUnit.Framework;
using SolDeCobre.Gameplay;
using SolDeCobre.Narrative;

namespace SolDeCobre.Tests
{
    public sealed class MissionProgressTests
    {
        static readonly string[] M01 =
        {
            "POCO_EXAMINADO", "LACO_APRENDIDO", "DESVIO_ENCONTRADO",
            "PATIO_ABERTO", "BOSS_01_DEFEATED", "LVL_01_COMPLETE"
        };

        [Test] public void InOrderFlagsCompleteTheMission()
        {
            var m = new MissionProgress(M01);
            for (int i = 0; i < M01.Length - 1; i++)
                Assert.AreEqual(MissionEventResult.Advanced, m.Report(M01[i]));
            Assert.AreEqual("LVL_01_COMPLETE", m.NextExpected);
            Assert.AreEqual(MissionEventResult.Completed, m.Report("LVL_01_COMPLETE"));
            Assert.IsTrue(m.Completed);
            Assert.IsNull(m.NextExpected);
        }
        [Test] public void OutOfOrderIsRejected()
        {
            var m = new MissionProgress(M01);
            Assert.AreEqual(MissionEventResult.Rejected, m.Report("BOSS_01_DEFEATED"));
            Assert.AreEqual(0, m.CompletedCount);
        }
        [Test] public void RepeatedAndLateEventsAreSafeNoOps()
        {
            var m = new MissionProgress(M01);
            Assert.AreEqual(MissionEventResult.Advanced, m.Report("POCO_EXAMINADO"));
            Assert.AreEqual(MissionEventResult.Duplicate, m.Report("POCO_EXAMINADO"));
            // Completa e então recebe um evento anterior atrasado.
            foreach (var f in new[] { "LACO_APRENDIDO", "DESVIO_ENCONTRADO", "PATIO_ABERTO", "BOSS_01_DEFEATED", "LVL_01_COMPLETE" })
                m.Report(f);
            Assert.IsTrue(m.Completed);
            Assert.AreEqual(MissionEventResult.Duplicate, m.Report("BOSS_01_DEFEATED"));
        }
        [Test] public void RewardIsGrantedOnlyOnce()
        {
            var m = new MissionProgress(M01);
            foreach (var f in M01) m.Report(f);
            Assert.IsTrue(m.TryGrantReward());
            Assert.IsFalse(m.TryGrantReward());
        }
        [Test] public void RestoreAdvancesByLongestOrderedPrefix()
        {
            var m = new MissionProgress(M01);
            m.Restore(new HashSet<string> { "POCO_EXAMINADO", "LACO_APRENDIDO", "PATIO_ABERTO" }, false);
            Assert.AreEqual(2, m.CompletedCount); // para em DESVIO (ausente); PATIO fora de ordem ignorado
            Assert.AreEqual("DESVIO_ENCONTRADO", m.NextExpected);
        }
    }

    public sealed class HeroVitalityTests
    {
        [Test] public void FiveHitsOfOneKill()
        {
            var h = new HeroVitality(5f);
            for (int i = 0; i < 4; i++) Assert.IsFalse(h.TakeDamage(1f));
            Assert.IsTrue(h.TakeDamage(1f));
            Assert.IsTrue(h.IsDead);
            h.Reset();
            Assert.IsFalse(h.IsDead);
        }
        [Test] public void AssistHalfDamageDoublesHits()
        {
            var h = new HeroVitality(5f);
            for (int i = 0; i < 9; i++) Assert.IsFalse(h.TakeDamage(0.5f));
            Assert.IsTrue(h.TakeDamage(0.5f));
        }
    }

    public sealed class EnemyThreatCycleTests
    {
        static EnemyThreatCycle Hover() => new EnemyThreatCycle(1, 1.0f, 0.5f, 1.5f);

        [Test] public void DamageOnlyDuringActiveAndGapEnforced()
        {
            var e = Hover(); e.Activate(); e.SetAggro(true);
            e.Tick(1.5f); // consome gap inicial e entra em Prepare
            Assert.AreEqual(EnemyPhase.Prepare, e.Phase);
            Assert.IsFalse(e.DealsDamageNow);
            e.Tick(1.0f); Assert.AreEqual(EnemyPhase.Active, e.Phase);
            Assert.IsTrue(e.DealsDamageNow);
            e.Tick(0.5f); Assert.AreEqual(EnemyPhase.Recover, e.Phase);
            Assert.IsFalse(e.DealsDamageNow);
            e.Tick(1.5f); Assert.AreEqual(EnemyPhase.Idle, e.Phase);
        }
        [Test] public void PauseFreezesTimers()
        {
            var e = Hover(); e.Activate(); e.SetAggro(true);
            e.Freeze(true);
            e.Tick(10f);
            Assert.AreEqual(EnemyPhase.Idle, e.Phase);
            e.Freeze(false); e.Tick(1.5f);
            Assert.AreEqual(EnemyPhase.Prepare, e.Phase);
        }
        [Test] public void ValidHitDefeatsAndPublishesOnce()
        {
            var e = new EnemyThreatCycle(1, 1f, 0.5f, 1.5f); e.Activate();
            Assert.IsTrue(e.TakeValidHit());
            Assert.AreEqual(EnemyPhase.Defeated, e.Phase);
            Assert.IsTrue(e.TryPublishDefeat());
            Assert.IsFalse(e.TryPublishDefeat());
        }
        [Test] public void AssistExtendsWarning()
        {
            var e = new EnemyThreatCycle(1, 1f, 0.5f, 1.5f, 1.5f, assist: true);
            e.Activate(); e.SetAggro(true);
            e.Tick(1.5f); // gap
            e.Tick(1.0f); Assert.AreEqual(EnemyPhase.Prepare, e.Phase); // 1.0 < 1.35
            e.Tick(0.4f); Assert.AreEqual(EnemyPhase.Active, e.Phase);
        }
    }

    public sealed class BossEncounterTests
    {
        static void AdvanceToPhase(BossEncounter b, BossPhase target, float max = 30f)
        {
            float t = 0f;
            while (b.Phase != target && t < max) { b.Tick(0.05f); t += 0.05f; }
            Assert.AreEqual(target, b.Phase, "não alcançou a fase esperada");
        }

        [Test] public void ThreeExposuresDefeatWithIdempotentVictory()
        {
            var b = new BossEncounter();
            b.Begin();
            AdvanceToPhase(b, BossPhase.Exposed);
            Assert.AreEqual(LassoResult.Scored, b.TryLasso());
            Assert.AreEqual(1, b.ExposuresDone);
            Assert.AreEqual(2, b.DifficultyPhase);

            AdvanceToPhase(b, BossPhase.Exposed);
            Assert.AreEqual(LassoResult.Scored, b.TryLasso());
            Assert.AreEqual(3, b.DifficultyPhase);

            AdvanceToPhase(b, BossPhase.Exposed);
            Assert.AreEqual(LassoResult.Defeated, b.TryLasso());
            Assert.AreEqual(BossPhase.Defeated, b.Phase);
            // Idempotente: reentrada/ataque após vitória não conta de novo.
            Assert.AreEqual(LassoResult.AlreadyDefeated, b.TryLasso());
            Assert.IsTrue(b.TryPublishDefeat());
            Assert.IsFalse(b.TryPublishDefeat());
        }
        [Test] public void ArmoredOutsideExposedWindow()
        {
            var b = new BossEncounter();
            b.Begin();
            Assert.AreEqual(LassoResult.ArmoredFeedback, b.TryLasso()); // Intro
            AdvanceToPhase(b, BossPhase.Telegraph);
            Assert.AreEqual(LassoResult.ArmoredFeedback, b.TryLasso());
            Assert.AreEqual(0, b.ExposuresDone);
        }
        [Test] public void MissedExposedWindowDoesNotConsumeExposure()
        {
            var b = new BossEncounter();
            b.Begin();
            AdvanceToPhase(b, BossPhase.Exposed);
            // Deixa a janela passar sem laço.
            for (int i = 0; i < 40 && b.Phase == BossPhase.Exposed; i++) b.Tick(0.05f);
            Assert.AreNotEqual(BossPhase.Exposed, b.Phase);
            Assert.AreEqual(0, b.ExposuresDone);
        }
        [Test] public void OnlyActiveDealsHeroDamageAndPauseFreezes()
        {
            var b = new BossEncounter();
            b.Begin();
            AdvanceToPhase(b, BossPhase.Active);
            Assert.IsTrue(b.DealsHeroDamageNow);
            var phase = b.Phase;
            b.Freeze(true); b.Tick(5f);
            Assert.AreEqual(phase, b.Phase); // pausa congela timers
            b.Freeze(false);
            AdvanceToPhase(b, BossPhase.Exposed);
            Assert.IsFalse(b.DealsHeroDamageNow);
        }
        [Test] public void CheckpointRestoreKeepsBankedExposures()
        {
            var b = new BossEncounter();
            b.Begin();
            AdvanceToPhase(b, BossPhase.Exposed);
            b.TryLasso(); // 1 exposição concluída, fase 2
            AdvanceToPhase(b, BossPhase.Active);
            b.RestoreCurrentPhase();
            Assert.AreEqual(1, b.ExposuresDone);        // mantém o que já foi batido
            Assert.AreEqual(2, b.DifficultyPhase);
            Assert.AreEqual(BossPhase.Telegraph, b.Phase); // recomeça o ciclo da fase
        }
    }
}
