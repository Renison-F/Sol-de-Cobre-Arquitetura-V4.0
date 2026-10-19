using System;
namespace SolDeCobre.Security
{
    public enum LicenseMode { NeedsActivation, Valid, Grace, NeedsRenewal, ClockReview }
    // Only an authenticated verifier adapter may feed this policy. Never parse raw JSON here.
    public readonly struct LicenseWindow
    {
        public readonly long IssuedAt, ExpiresAt, OfflineUntil;
        public LicenseWindow(long iat, long exp, long until) { IssuedAt=iat; ExpiresAt=exp; OfflineUntil=until; }
    }
    public static class LicensePolicy
    {
        public static LicenseMode Evaluate(LicenseWindow window, long now, long lastTrustedTime)
        {
            if (now + 300 < lastTrustedTime) return LicenseMode.ClockReview;
            if (window.IssuedAt < 0 || window.IssuedAt > now+300 ||
                window.ExpiresAt <= window.IssuedAt || window.OfflineUntil < window.ExpiresAt ||
                window.OfflineUntil-window.IssuedAt > 37L*86400L)
                return LicenseMode.NeedsActivation;
            if (now >= window.OfflineUntil) return LicenseMode.NeedsRenewal;
            return now >= window.ExpiresAt ? LicenseMode.Grace : LicenseMode.Valid;
        }
    }
}
