namespace SolDeCobre.Security
{
    // Renovação oportunista: após 24 h com rede, não a cada frame/fase (SEGURANCA.md §5).
    // Puro e determinístico; o tempo e o estado de rede vêm de fora.
    public static class RenewalScheduler
    {
        public const long DefaultIntervalSeconds = 24L * 3600L;

        public static bool ShouldRenew(long nowUnix, long lastRenewUnix, bool hasNetwork,
            bool renewalInFlight, long minIntervalSeconds = DefaultIntervalSeconds)
        {
            if (!hasNetwork || renewalInFlight) return false;
            if (minIntervalSeconds < 0) minIntervalSeconds = 0;
            return nowUnix - lastRenewUnix >= minIntervalSeconds;
        }
    }
}
