using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using SolDeCobre.Gameplay;
using SolDeCobre.Security;

namespace SolDeCobre.Tests
{
    public sealed class HeroSwitcherTests
    {
        [Test] public void LockedSecondHeroRejectsSwitch()
        {
            var s = new HeroSwitcher("mara", unlocked: false);
            Assert.AreEqual(SwitchResult.Rejected, s.Request(grounded: true, blocked: false));
            Assert.AreEqual("mara", s.CurrentHero);
        }
        [Test] public void GroundedSafeSwitchTogglesHero()
        {
            var s = new HeroSwitcher("mara", unlocked: true);
            Assert.AreEqual(SwitchResult.Switched, s.Request(true, false));
            Assert.AreEqual("ze", s.CurrentHero);
        }
        [Test] public void BlockedStateRejectsWithoutQueue()
        {
            var s = new HeroSwitcher("mara", unlocked: true);
            Assert.AreEqual(SwitchResult.Rejected, s.Request(grounded: true, blocked: true)); // hit/crítico/resgate
            Assert.IsFalse(s.HasQueued);
        }
        [Test] public void AirborneQueuesAndExecutesOnSafeGround()
        {
            var s = new HeroSwitcher("mara", unlocked: true);
            Assert.AreEqual(SwitchResult.Queued, s.Request(grounded: false, blocked: false));
            Assert.IsTrue(s.HasQueued);
            Assert.AreEqual(SwitchResult.Switched, s.Tick(0.1f, grounded: true, blocked: false));
            Assert.AreEqual("ze", s.CurrentHero);
        }
        [Test] public void QueueExpiresOrCancelsWithoutSwitch()
        {
            var s = new HeroSwitcher("mara", unlocked: true);
            s.Request(false, false);
            Assert.AreEqual(SwitchResult.Dropped, s.Tick(0.4f, grounded: false, blocked: false)); // passou de 0,3 s
            Assert.AreEqual("mara", s.CurrentHero);

            var t = new HeroSwitcher("mara", unlocked: true);
            t.Request(false, false);
            Assert.AreEqual(SwitchResult.Dropped, t.Tick(0.1f, grounded: false, blocked: true)); // bloqueou no meio
            Assert.AreEqual("mara", t.CurrentHero);
        }
    }

    public sealed class LicenseRenewalServiceTests
    {
        sealed class FakeProof : IPlatformProofProvider
        {
            public Task<string> CreateProofAsync(string requestHash, CancellationToken c) => Task.FromResult("proof:" + requestHash);
        }
        sealed class FakeTransport : ILicenseTransport
        {
            readonly string token; readonly bool throws;
            public FakeTransport(string token, bool throws = false) { this.token = token; this.throws = throws; }
            public Task<string> RenewAsync(string challenge, string proof, CancellationToken c) =>
                throws ? Task.FromException<string>(new InvalidOperationException()) : Task.FromResult(token);
        }
        sealed class FakeVerifier : ILicenseVerifier
        {
            readonly bool throws;
            public FakeVerifier(bool throws = false) { this.throws = throws; }
            public LicenseWindow Verify(string token, string installation)
            {
                if (throws) throw new System.Security.Cryptography.CryptographicException("fake");
                return new LicenseWindow(1000, 2000, 3000);
            }
        }
        sealed class RecordingStorage : ISecureStorage
        {
            public bool Wrote;
            public Task<byte[]> ReadAsync(string key, CancellationToken c) => Task.FromResult<byte[]>(null);
            public Task WriteAtomicAsync(string key, byte[] value, CancellationToken c) { Wrote = true; return Task.CompletedTask; }
        }

        static LicenseRenewalService Make(ILicenseTransport t, ILicenseVerifier v, RecordingStorage s) =>
            new LicenseRenewalService(new FakeProof(), t, v, s, "install-1");

        [Test] public async Task NoNetworkDoesNotRenewOrWrite()
        {
            var s = new RecordingStorage();
            var svc = Make(new FakeTransport("tok"), new FakeVerifier(), s);
            Assert.AreEqual(RenewOutcome.NoNetwork, await svc.TryRenewAsync("chal", hasNetwork: false, CancellationToken.None));
            Assert.IsFalse(s.Wrote);
        }
        [Test] public async Task HappyPathRenewsAndStores()
        {
            var s = new RecordingStorage();
            var svc = Make(new FakeTransport("tok"), new FakeVerifier(), s);
            Assert.AreEqual(RenewOutcome.Renewed, await svc.TryRenewAsync("chal", true, CancellationToken.None));
            Assert.IsTrue(s.Wrote);
        }
        [Test] public async Task UnverifiedEnvelopeIsNeverStored()
        {
            var s = new RecordingStorage();
            var svc = Make(new FakeTransport("tok"), new FakeVerifier(throws: true), s);
            Assert.AreEqual(RenewOutcome.Failed, await svc.TryRenewAsync("chal", true, CancellationToken.None));
            Assert.IsFalse(s.Wrote); // verifica antes de guardar
        }
        [Test] public async Task TransportFailureDoesNotExtend()
        {
            var s = new RecordingStorage();
            var svc = Make(new FakeTransport(null, throws: true), new FakeVerifier(), s);
            Assert.AreEqual(RenewOutcome.Failed, await svc.TryRenewAsync("chal", true, CancellationToken.None));
            Assert.IsFalse(s.Wrote);
        }
    }
}
