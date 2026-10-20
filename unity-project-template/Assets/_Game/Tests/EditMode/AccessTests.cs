using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using SolDeCobre.Security;

namespace SolDeCobre.Tests
{
    public sealed class AccessGateTests
    {
        [Test] public void NeverActivatedNeedsActivation()
        {
            var d = AccessGate.Evaluate(LicenseMode.Valid, everActivated: false);
            Assert.AreEqual(AccessState.NeedsActivation, d.State);
            Assert.IsFalse(d.AllowStartSession);
        }
        [Test] public void ValidAndGraceAllowPlay()
        {
            Assert.AreEqual(AccessState.PlayAllowed, AccessGate.Evaluate(LicenseMode.Valid, true).State);
            var grace = AccessGate.Evaluate(LicenseMode.Grace, true);
            Assert.AreEqual(AccessState.GracePlayAllowed, grace.State);
            Assert.IsTrue(grace.AllowStartSession);
        }
        [Test] public void RenewalBlocksNewSessionButReachesCheckpoint()
        {
            var d = AccessGate.Evaluate(LicenseMode.NeedsRenewal, true);
            Assert.AreEqual(AccessState.NeedsRenewal, d.State);
            Assert.IsFalse(d.AllowStartSession);
            Assert.IsTrue(d.AllowContinueToCheckpoint); // não interrompe um salto
        }
        [Test] public void ClockReviewBlocksStart()
        {
            var d = AccessGate.Evaluate(LicenseMode.ClockReview, true);
            Assert.AreEqual(AccessState.ClockReview, d.State);
            Assert.IsFalse(d.AllowStartSession);
        }
    }

    public sealed class RenewalSchedulerTests
    {
        [Test] public void RenewsAfter24hWithNetwork()
        {
            Assert.IsTrue(RenewalScheduler.ShouldRenew(100000, 100000 - 86400, true, false));
        }
        [Test] public void NoNetworkOrInFlightOrTooSoonDoesNotRenew()
        {
            Assert.IsFalse(RenewalScheduler.ShouldRenew(100000, 0, hasNetwork: false, renewalInFlight: false));
            Assert.IsFalse(RenewalScheduler.ShouldRenew(100000, 0, true, renewalInFlight: true));
            Assert.IsFalse(RenewalScheduler.ShouldRenew(100000, 100000 - 3600, true, false)); // 1 h só
        }
    }

    public sealed class AccessCoordinatorTests
    {
        sealed class FakeStorage : ISecureStorage
        {
            readonly byte[] data; readonly bool throws;
            public FakeStorage(byte[] data, bool throws = false) { this.data = data; this.throws = throws; }
            public Task<byte[]> ReadAsync(string key, CancellationToken c) =>
                throws ? Task.FromException<byte[]>(new InvalidOperationException()) : Task.FromResult(data);
            public Task WriteAtomicAsync(string key, byte[] value, CancellationToken c) => Task.CompletedTask;
        }
        sealed class FakeVerifier : ILicenseVerifier
        {
            readonly LicenseWindow window; readonly bool throws;
            public FakeVerifier(LicenseWindow w, bool throws = false) { window = w; this.throws = throws; }
            public LicenseWindow Verify(string token, string expectedInstallation)
            {
                if (throws) throw new System.Security.Cryptography.CryptographicException("fake");
                return window;
            }
        }
        static AccessCoordinator Make(ISecureStorage s, ILicenseVerifier v, long now, long lastTrusted) =>
            new AccessCoordinator(s, v, "install-1", () => now, () => lastTrusted);

        static readonly byte[] Token = System.Text.Encoding.ASCII.GetBytes("envelope");

        [Test] public async Task NoStoredTokenNeedsActivation()
        {
            var c = Make(new FakeStorage(null), new FakeVerifier(new LicenseWindow(1000, 2000, 3000)), 1500, 1000);
            var d = await c.EvaluateAsync(CancellationToken.None);
            Assert.AreEqual(AccessState.NeedsActivation, d.State);
            Assert.IsFalse(d.AllowStartSession);
        }
        [Test] public async Task ValidEnvelopeAllowsPlay()
        {
            var c = Make(new FakeStorage(Token), new FakeVerifier(new LicenseWindow(1000, 2000, 3000)), 1500, 1000);
            Assert.AreEqual(AccessState.PlayAllowed, (await c.EvaluateAsync(CancellationToken.None)).State);
        }
        [Test] public async Task GraceWindowReportsGrace()
        {
            var c = Make(new FakeStorage(Token), new FakeVerifier(new LicenseWindow(1000, 2000, 3000)), 2500, 1000);
            Assert.AreEqual(AccessState.GracePlayAllowed, (await c.EvaluateAsync(CancellationToken.None)).State);
        }
        [Test] public async Task PastOfflineCapNeedsRenewal()
        {
            var c = Make(new FakeStorage(Token), new FakeVerifier(new LicenseWindow(1000, 2000, 3000)), 3500, 1000);
            Assert.AreEqual(AccessState.NeedsRenewal, (await c.EvaluateAsync(CancellationToken.None)).State);
        }
        [Test] public async Task TamperedEnvelopeNeverReleasesCampaign()
        {
            var c = Make(new FakeStorage(Token), new FakeVerifier(default, throws: true), 1500, 1000);
            var d = await c.EvaluateAsync(CancellationToken.None);
            Assert.AreEqual(AccessState.NeedsActivation, d.State);
            Assert.IsFalse(d.AllowStartSession);
        }
    }
}
