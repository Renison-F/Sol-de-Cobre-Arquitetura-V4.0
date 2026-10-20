using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace SolDeCobre.Security
{
    // Orquestra a decisão de acesso no boot: lê o envelope guardado, verifica
    // criptograficamente (assinatura, vínculos, atualidade), aplica a política de prazo
    // e o portão. Nunca libera campanha a partir de conteúdo não verificado; token
    // ausente ou inválido => NeedsActivation (reativar, sem estender validade localmente).
    public sealed class AccessCoordinator
    {
        public const string TokenKey = "license.envelope";

        readonly ISecureStorage storage;
        readonly ILicenseVerifier verifier;
        readonly string installation;
        readonly Func<long> clock;
        readonly Func<long> lastTrustedClock;

        public AccessCoordinator(ISecureStorage storage, ILicenseVerifier verifier,
            string installation, Func<long> clock, Func<long> lastTrustedClock = null)
        {
            this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
            this.verifier = verifier ?? throw new ArgumentNullException(nameof(verifier));
            this.installation = installation;
            this.clock = clock ?? throw new ArgumentNullException(nameof(clock));
            this.lastTrustedClock = lastTrustedClock ?? clock;
        }

        public async Task<AccessDecision> EvaluateAsync(CancellationToken cancellation)
        {
            byte[] raw;
            try { raw = await storage.ReadAsync(TokenKey, cancellation).ConfigureAwait(false); }
            catch (Exception) { return AccessGate.Evaluate(LicenseMode.NeedsActivation, false); }

            if (raw == null || raw.Length == 0)
                return AccessGate.Evaluate(LicenseMode.NeedsActivation, false); // nunca ativado

            LicenseWindow window;
            try { window = verifier.Verify(Encoding.ASCII.GetString(raw), installation); }
            catch (Exception)
            {
                // Envelope inválido/adulterado: exige reativação; jamais libera campanha.
                return AccessGate.Evaluate(LicenseMode.NeedsActivation, true);
            }

            var mode = LicensePolicy.Evaluate(window, clock(), lastTrustedClock());
            return AccessGate.Evaluate(mode, true);
        }
    }
}
