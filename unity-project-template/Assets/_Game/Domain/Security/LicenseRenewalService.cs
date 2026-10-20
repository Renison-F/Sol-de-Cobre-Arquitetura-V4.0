using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace SolDeCobre.Security
{
    public enum RenewOutcome { Renewed, NoNetwork, Failed }

    // Caso de uso RenewAccess (modulos-e-contratos.md): obtém prova de plataforma,
    // pede renovação ao serviço, VERIFICA o envelope e só então o guarda. Falha de rede
    // ou verificação NÃO estende prazo por conta própria — preserva o envelope anterior.
    public sealed class LicenseRenewalService
    {
        readonly IPlatformProofProvider proof;
        readonly ILicenseTransport transport;
        readonly ILicenseVerifier verifier;
        readonly ISecureStorage storage;
        readonly string installation;

        public LicenseRenewalService(IPlatformProofProvider proof, ILicenseTransport transport,
            ILicenseVerifier verifier, ISecureStorage storage, string installation)
        {
            this.proof = proof ?? throw new ArgumentNullException(nameof(proof));
            this.transport = transport ?? throw new ArgumentNullException(nameof(transport));
            this.verifier = verifier ?? throw new ArgumentNullException(nameof(verifier));
            this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
            this.installation = installation;
        }

        public async Task<RenewOutcome> TryRenewAsync(string challenge, bool hasNetwork, CancellationToken cancellation)
        {
            if (!hasNetwork || string.IsNullOrEmpty(challenge)) return RenewOutcome.NoNetwork;
            try
            {
                string requestHash = Sha256Hex(challenge);
                string platformProof = await proof.CreateProofAsync(requestHash, cancellation).ConfigureAwait(false);
                string token = await transport.RenewAsync(challenge, platformProof, cancellation).ConfigureAwait(false);
                // Verifica ANTES de guardar: nunca persiste envelope não verificado.
                verifier.Verify(token, installation);
                await storage.WriteAtomicAsync(AccessCoordinator.TokenKey,
                    Encoding.ASCII.GetBytes(token), cancellation).ConfigureAwait(false);
                return RenewOutcome.Renewed;
            }
            catch (Exception)
            {
                return RenewOutcome.Failed; // preserva o anterior; não estende prazo
            }
        }

        static string Sha256Hex(string value)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(value));
                var sb = new StringBuilder(bytes.Length * 2);
                foreach (var b in bytes) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
    }
}
