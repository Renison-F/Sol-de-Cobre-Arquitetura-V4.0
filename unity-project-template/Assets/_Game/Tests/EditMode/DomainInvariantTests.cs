using NUnit.Framework;
using SolDeCobre.Core;
using SolDeCobre.Narrative;
using SolDeCobre.Security;

namespace SolDeCobre.Tests
{
    public sealed class DomainInvariantTests
    {
        [Test] public void RepeatedObjectiveDoesNotDuplicateProgress()
        {
            var state = new QuestState();
            Assert.IsTrue(state.GrantFlag("PATIO_ABERTO"));
            Assert.IsFalse(state.GrantFlag("PATIO_ABERTO"));
            Assert.AreEqual(1, state.Snapshot().Length);
            Assert.IsFalse(state.Ready(new[] { "PATIO_ABERTO", "BOSS_01_DEFEATED" }));
            state.GrantFlag("BOSS_01_DEFEATED");
            Assert.IsTrue(state.Ready(new[] { "PATIO_ABERTO", "BOSS_01_DEFEATED" }));
        }
        [Test] public void SnapshotCannotMutateQuestState()
        {
            var state = new QuestState(); state.GrantFlag("POCO_EXAMINADO");
            var snapshot = state.Snapshot(); snapshot[0] = "LVL_10_COMPLETE";
            Assert.IsTrue(state.HasFlag("POCO_EXAMINADO"));
            Assert.IsFalse(state.HasFlag("LVL_10_COMPLETE"));
        }
        [Test] public void SessionCannotStartPlayingBeforeLoading()
        {
            var phase = new GamePhaseMachine();
            Assert.IsFalse(phase.TryTransition(GamePhase.Playing));
            Assert.AreEqual(GamePhase.Boot, phase.Current);
            Assert.IsTrue(phase.TryTransition(GamePhase.Menu));
            Assert.IsTrue(phase.TryTransition(GamePhase.Loading));
            Assert.IsTrue(phase.TryTransition(GamePhase.Playing));
        }
        [Test] public void GraceDoesNotExtendPastAuthenticatedWindow()
        {
            var window = new LicenseWindow(1000, 2000, 3000);
            Assert.AreEqual(LicenseMode.Valid, LicensePolicy.Evaluate(window, 1999, 1000));
            Assert.AreEqual(LicenseMode.Grace, LicensePolicy.Evaluate(window, 2000, 1000));
            Assert.AreEqual(LicenseMode.NeedsRenewal, LicensePolicy.Evaluate(window, 3000, 1000));
            Assert.AreEqual(LicenseMode.ClockReview, LicensePolicy.Evaluate(window, 1200, 2000));
        }
    }
}
