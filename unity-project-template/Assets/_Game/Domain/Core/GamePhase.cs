namespace SolDeCobre.Core
{
    public enum GamePhase { Boot, Menu, Loading, Playing, Paused, Results }
    public sealed class GamePhaseMachine
    {
        public GamePhase Current { get; private set; } = GamePhase.Boot;
        public bool TryTransition(GamePhase destination)
        {
            if (Current == destination) return false;
            bool permitted = (Current, destination) switch
            {
                (GamePhase.Boot, GamePhase.Menu) => true,
                (GamePhase.Menu, GamePhase.Loading) => true,
                (GamePhase.Loading, GamePhase.Playing) => true,
                (GamePhase.Playing, GamePhase.Paused) => true,
                (GamePhase.Paused, GamePhase.Playing) => true,
                (GamePhase.Playing, GamePhase.Results) => true,
                (GamePhase.Results, GamePhase.Menu) => true,
                _ => false
            };
            if (permitted) Current=destination;
            return permitted;
        }
    }
}
