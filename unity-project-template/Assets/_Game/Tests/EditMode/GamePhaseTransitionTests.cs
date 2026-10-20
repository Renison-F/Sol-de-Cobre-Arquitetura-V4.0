using NUnit.Framework;
using SolDeCobre.Core;

namespace SolDeCobre.Tests
{
    // DEV-02: recuperação de carga, cancelamento e encerramento de sessão.
    public sealed class GamePhaseTransitionTests
    {
        static GamePhaseMachine AtLoading()
        {
            var phase = new GamePhaseMachine();
            Assert.IsTrue(phase.TryTransition(GamePhase.Menu));
            Assert.IsTrue(phase.TryTransition(GamePhase.Loading));
            return phase;
        }
        static GamePhaseMachine AtPlaying()
        {
            var phase = AtLoading();
            Assert.IsTrue(phase.TryTransition(GamePhase.Playing));
            return phase;
        }

        [Test] public void FailedLoadRecoversToMenu()
        {
            var phase = AtLoading();
            Assert.IsTrue(phase.CancelLoading());
            Assert.AreEqual(GamePhase.Menu, phase.Current);
        }
        [Test] public void CancelLoadingOnlyValidWhileLoading()
        {
            var phase = AtPlaying();
            Assert.IsFalse(phase.CancelLoading());
            Assert.AreEqual(GamePhase.Playing, phase.Current);
        }
        [Test] public void EndSessionFromPlayingReturnsToMenu()
        {
            var phase = AtPlaying();
            Assert.IsTrue(phase.EndSession());
            Assert.AreEqual(GamePhase.Menu, phase.Current);
        }
        [Test] public void EndSessionFromPausedReturnsToMenu()
        {
            var phase = AtPlaying();
            Assert.IsTrue(phase.TryTransition(GamePhase.Paused));
            Assert.IsTrue(phase.EndSession());
            Assert.AreEqual(GamePhase.Menu, phase.Current);
        }
        [Test] public void EndSessionFromMenuIsRejected()
        {
            var phase = new GamePhaseMachine();
            Assert.IsTrue(phase.TryTransition(GamePhase.Menu));
            Assert.IsFalse(phase.EndSession());
            Assert.AreEqual(GamePhase.Menu, phase.Current);
        }
        [Test] public void ResultsStillRoutesBackToMenu()
        {
            var phase = AtPlaying();
            Assert.IsTrue(phase.TryTransition(GamePhase.Results));
            Assert.IsTrue(phase.TryTransition(GamePhase.Menu));
            Assert.AreEqual(GamePhase.Menu, phase.Current);
        }
    }
}
