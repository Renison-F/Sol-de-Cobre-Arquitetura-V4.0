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
                // DEV-02: carga que falha ou é cancelada volta ao menu sem campanha falsa.
                (GamePhase.Loading, GamePhase.Menu) => true,
                (GamePhase.Playing, GamePhase.Paused) => true,
                (GamePhase.Paused, GamePhase.Playing) => true,
                // DEV-02: encerrar sessão a partir do jogo ou da pausa retorna ao menu.
                (GamePhase.Playing, GamePhase.Menu) => true,
                (GamePhase.Paused, GamePhase.Menu) => true,
                (GamePhase.Playing, GamePhase.Results) => true,
                (GamePhase.Results, GamePhase.Menu) => true,
                _ => false
            };
            if (permitted) Current=destination;
            return permitted;
        }

        // DEV-02: cancelamento de carga. Só é válido enquanto a sessão não começou a jogar.
        public bool CancelLoading() => Current == GamePhase.Loading && TryTransition(GamePhase.Menu);

        // DEV-02: encerrar a sessão em andamento (jogo ou pausa) e voltar ao menu.
        public bool EndSession() =>
            (Current == GamePhase.Playing || Current == GamePhase.Paused) && TryTransition(GamePhase.Menu);
    }
}
