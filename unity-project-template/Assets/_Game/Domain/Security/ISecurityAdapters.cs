using System;
using System.Threading;
using System.Threading.Tasks;
namespace SolDeCobre.Security
{
    // Implement with Play Integrity / App Attest plugins after store project setup.
    public interface IPlatformProofProvider
    {
        Task<string> CreateProofAsync(string requestHash, CancellationToken cancellation);
    }
    // Secure storage protects tokens and time anchors; never contains server signing secrets.
    public interface ISecureStorage
    {
        Task<byte[]> ReadAsync(string key, CancellationToken cancellation);
        Task WriteAtomicAsync(string key, byte[] value, CancellationToken cancellation);
    }
    public interface ILicenseVerifier
    {
        // Must verify signature, fixed envelope version, allowlisted kid, all claims,
        // expected app/platform/installation, lease cap, revocation, then return window.
        // Throw on failure. Never create a window from unverified token content.
        LicenseWindow Verify(string token, string expectedInstallation);
    }
    public interface ILicenseTransport
    {
        Task<string> RenewAsync(string challenge, string opaquePlatformProof, CancellationToken cancellation);
    }
}
