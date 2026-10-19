using NUnit.Framework;
using SolDeCobre.Gameplay;

namespace SolDeCobre.Tests
{
    public sealed class TargetSelectionTests
    {
        [Test] public void OutOfRangeOrConeIsNotSelectable()
        {
            var list = new[]
            {
                new AbilityTarget("A", 9f, 5f, false),   // fora de alcance (7 m)
                new AbilityTarget("B", 3f, 40f, false),  // fora do cone (30°)
            };
            Assert.IsFalse(TargetSelection.TrySelect(list, 7f, 30f, out _));
        }
        [Test] public void MissionObjectWinsOverNearerPlainTarget()
        {
            var list = new[]
            {
                new AbilityTarget("plain", 1f, 2f, false),
                new AbilityTarget("mission", 5f, 10f, true),
            };
            Assert.IsTrue(TargetSelection.TrySelect(list, 7f, 30f, out var chosen));
            Assert.AreEqual("mission", chosen.Id);
        }
        [Test] public void MostCentralAmongEquals()
        {
            var list = new[]
            {
                new AbilityTarget("wide", 3f, 20f, false),
                new AbilityTarget("center", 3f, 4f, false),
            };
            Assert.IsTrue(TargetSelection.TrySelect(list, 7f, 30f, out var chosen));
            Assert.AreEqual("center", chosen.Id);
        }
        [Test] public void StableIdBreaksTies()
        {
            var list = new[]
            {
                new AbilityTarget("beta", 3f, 10f, false),
                new AbilityTarget("alpha", 3f, 10f, false),
            };
            Assert.IsTrue(TargetSelection.TrySelect(list, 7f, 30f, out var chosen));
            Assert.AreEqual("alpha", chosen.Id);
        }
    }

    public sealed class AbilityControllerTests
    {
        static AbilityController Make() => new AbilityController(0.1f, 0.2f, 1.0f);

        [Test] public void FullCycleAppliesEffectThenCoolsDown()
        {
            var a = Make();
            Assert.IsTrue(a.Begin(true));
            Assert.AreEqual(AbilityPhase.Windup, a.Phase);
            a.Tick(0.15f);
            Assert.AreEqual(AbilityPhase.Active, a.Phase);
            a.Tick(0.25f);
            Assert.AreEqual(AbilityPhase.Recovery, a.Phase);
            Assert.IsTrue(a.EffectAppliedThisCycle);
            Assert.IsTrue(a.OnCooldown);
            Assert.IsFalse(a.Begin(true)); // não inicia em cooldown
            a.Tick(1.0f);
            Assert.AreEqual(AbilityPhase.Idle, a.Phase);
            Assert.IsTrue(a.IsReady);
        }
        [Test] public void NoTargetMeansShortWindupNoCostNoCooldown()
        {
            var a = Make();
            Assert.IsTrue(a.Begin(false));
            a.Tick(0.15f);
            Assert.AreEqual(AbilityPhase.Idle, a.Phase);
            Assert.IsFalse(a.EffectAppliedThisCycle);
            Assert.IsFalse(a.OnCooldown);
        }
        [Test] public void CancelDuringActiveLeavesNoEffect()
        {
            var a = Make();
            a.Begin(true);
            a.Tick(0.15f);
            Assert.AreEqual(AbilityPhase.Active, a.Phase);
            Assert.IsTrue(a.Cancel());
            Assert.AreEqual(AbilityPhase.Idle, a.Phase);
            Assert.IsFalse(a.EffectAppliedThisCycle);
        }
        [Test] public void SingleLargeTickTraversesWindupAndActive()
        {
            var a = Make();
            a.Begin(true);
            a.Tick(0.35f); // 0.1 windup + 0.2 active consumidos
            Assert.AreEqual(AbilityPhase.Recovery, a.Phase);
            Assert.IsTrue(a.EffectAppliedThisCycle);
        }
    }

    public sealed class CompanionDispatchTests
    {
        [Test] public void DispatchRunsOutAndReturnsHome()
        {
            var f = new CompanionDispatch(1.5f, 2f);
            Assert.IsTrue(f.Dispatch(true));
            Assert.AreEqual(CompanionPhase.Outbound, f.Phase);
            f.Tick(1.6f);
            Assert.AreEqual(CompanionPhase.Returning, f.Phase);
            Assert.IsTrue(f.ActivatedThisTrip);
            f.Tick(2.1f);
            Assert.AreEqual(CompanionPhase.Home, f.Phase);
        }
        [Test] public void NoRouteRequestsHintAndStaysHome()
        {
            var f = new CompanionDispatch();
            Assert.IsFalse(f.Dispatch(false));
            Assert.AreEqual(CompanionPhase.Home, f.Phase);
            Assert.IsTrue(f.RouteHintRequested);
        }
        [Test] public void RecallReturnsToOwnerWithoutActivating()
        {
            var f = new CompanionDispatch(1.5f, 2f);
            f.Dispatch(true);
            f.Tick(0.75f);
            Assert.IsTrue(f.Recall());
            Assert.AreEqual(CompanionPhase.Returning, f.Phase);
            Assert.IsFalse(f.ActivatedThisTrip);
            f.Tick(2.1f);
            Assert.AreEqual(CompanionPhase.Home, f.Phase);
            Assert.IsFalse(f.ActivatedThisTrip);
        }
    }
}
