using System;
using System.Threading;
using System.Threading.Tasks;
using SolDeCobre.Security;
namespace SolDeCobre.Runtime.Security
{
    // Adaptadores PLACEHOLDER do acesso comercial. Existem para que o projeto compile e
    // o fluxo seja montado, mas FALHAM de forma explícita — nunca devolvem sucesso falso.
    // Substituir pelos adaptadores reais (loja/atestação/KMS/transporte) do proprietário
    // antes de qualquer build comercial. Sem mocks silenciosos de sucesso (SEGURANCA.md).
    sealed class NotProvisioned : Exception
    {
        public NotProvisioned(string component)
            : base(component + " não está configurado: adaptador comercial real do proprietário é obrigatório antes do release.") { }
    }

    public sealed class UnconfiguredEntitlement : IEntitlementVerification
    {
        public Task<string> CreatePurchaseProofAsync(string challenge, CancellationToken cancellation) =>
            Task.FromException<string>(new NotProvisioned("Verificação de compra (loja/atestação)"));
        public Task<string> RestoreAsync(string challenge, CancellationToken cancellation) =>
            Task.FromException<string>(new NotProvisioned("Restauração de compra"));
    }

    public sealed class UnconfiguredLicenseTransport : ILicenseTransport
    {
        public Task<string> RenewAsync(string challenge, string opaquePlatformProof, CancellationToken cancellation) =>
            Task.FromException<string>(new NotProvisioned("Transporte de licença (serviço de renovação)"));
    }

    public sealed class UnconfiguredPlatformProof : IPlatformProofProvider
    {
        public Task<string> CreateProofAsync(string requestHash, CancellationToken cancellation) =>
            Task.FromException<string>(new NotProvisioned("Prova de plataforma (Play Integrity / App Attest)"));
    }
}
