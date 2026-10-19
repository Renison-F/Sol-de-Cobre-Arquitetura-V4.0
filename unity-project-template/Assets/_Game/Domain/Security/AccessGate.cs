namespace SolDeCobre.Security
{
    public enum AccessState { PlayAllowed, GracePlayAllowed, NeedsActivation, NeedsRenewal, ClockReview }

    // Decisão de acesso para a UI/sessão. Separa "pode iniciar nova sessão" de "pode
    // continuar até o próximo checkpoint": uma sessão já iniciada chega ao próximo
    // checkpoint e o aviso não interrompe um salto (05-offline-restauracao-e-ux / SEGURANCA.md).
    public readonly struct AccessDecision
    {
        public readonly AccessState State;
        public readonly bool AllowStartSession;
        public readonly bool AllowContinueToCheckpoint;
        public readonly string Reason;
        public AccessDecision(AccessState state, bool start, bool continueToCheckpoint, string reason)
        { State = state; AllowStartSession = start; AllowContinueToCheckpoint = continueToCheckpoint; Reason = reason; }
    }

    // Mapeia a política de prazo (LicensePolicy) + se já houve ativação em decisão de
    // acesso. Nunca estende validade localmente; preserva saves; primeira ativação exige rede.
    public static class AccessGate
    {
        public static AccessDecision Evaluate(LicenseMode mode, bool everActivated)
        {
            if (!everActivated)
                return new AccessDecision(AccessState.NeedsActivation, false, false, "primeira ativação exige rede");
            switch (mode)
            {
                case LicenseMode.Valid:
                    return new AccessDecision(AccessState.PlayAllowed, true, true, "licença válida");
                case LicenseMode.Grace:
                    // Em tolerância: joga normalmente; renovação oportunista recomendada.
                    return new AccessDecision(AccessState.GracePlayAllowed, true, true, "tolerância; renovar quando houver rede");
                case LicenseMode.NeedsRenewal:
                    // Após o limite offline: não inicia nova sessão, mas a sessão atual
                    // chega ao próximo checkpoint; saves preservados; menu oferece reconexão.
                    return new AccessDecision(AccessState.NeedsRenewal, false, true, "limite offline atingido; reconectar");
                case LicenseMode.ClockReview:
                    return new AccessDecision(AccessState.ClockReview, false, true, "relógio inconsistente; revisar");
                default:
                    return new AccessDecision(AccessState.NeedsActivation, false, false, "ativação necessária");
            }
        }
    }
}
