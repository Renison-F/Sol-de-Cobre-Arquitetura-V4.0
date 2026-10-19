using System.Threading;
using System.Threading.Tasks;
namespace SolDeCobre.Security
{
    // Verificação de titularidade de compra via loja/atestação nativa. O servidor
    // autentica o vínculo (desafio/instalação/chave pública/operação) antes de criar
    // o StoreResult interno. NENHUM endpoint aceita "licensed:true" do cliente e não há
    // implementação que devolva sucesso sem backend real — sem prova de compra falsa.
    public interface IEntitlementVerification
    {
        // Devolve a prova opaca de compra vinculada ao desafio, para o emissor validar.
        Task<string> CreatePurchaseProofAsync(string challenge, CancellationToken cancellation);
        // Restauração de compra em novo aparelho/instalação, também vinculada ao desafio.
        Task<string> RestoreAsync(string challenge, CancellationToken cancellation);
    }
}
