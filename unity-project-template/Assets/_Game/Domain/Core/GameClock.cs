using System;
namespace SolDeCobre.Core
{
    // Fonte única de tempo de parede (Unix, UTC) para gravação de save e carimbos.
    // NÃO é a âncora de prazo comercial da licença (essa vem do serviço verificado).
    public static class GameClock
    {
        public static long NowUnix() => DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
}
