namespace SolDeCobre.Gameplay
{
    public interface IAbility
    {
        string AbilityId { get; }
        bool CanExecute();
        bool TryExecute();
        void Interrupt();
    }
}
