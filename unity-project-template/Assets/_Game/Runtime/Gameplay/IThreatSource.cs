using UnityEngine;
namespace SolDeCobre.Gameplay
{
    // Fonte de ameaca para o aplicador de dano do GameDirector: expõe SE o golpe está
    // ativo agora (janela Active), QUANTO de dano e a origem para medir alcance.
    // Dano nunca ocorre fora dessa janela (sem contato quando inativo).
    public interface IThreatSource
    {
        bool DamageActive { get; }
        float DamageAmount { get; }
        Transform Origin { get; }
    }
}
