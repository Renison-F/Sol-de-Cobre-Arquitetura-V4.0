using SolDeCobre.Gameplay;
namespace SolDeCobre.Hud
{
    // Snapshot imutável da HUD (telas-e-fluxos.md: vida, próximo objetivo curto, touch
    // contextual; legível a 720p/safe area). A view só desenha; a fonte da verdade é o jogo.
    public readonly struct HudSnapshot
    {
        public readonly float HeartsCurrent, HeartsMax;
        public readonly string NextObjective;  // ação concreta; sem exigir leitura de IDs
        public readonly string GlyphSet;
        public readonly ThreatCue Threat;
        public readonly bool HintAvailable;     // dica opt-in (sem cobrir o salto)
        public readonly bool Subtitles;

        public HudSnapshot(float heartsCurrent, float heartsMax, string nextObjective,
            string glyphSet, ThreatCue threat, bool hintAvailable, bool subtitles)
        {
            HeartsCurrent = heartsCurrent; HeartsMax = heartsMax; NextObjective = nextObjective;
            GlyphSet = glyphSet; Threat = threat; HintAvailable = hintAvailable; Subtitles = subtitles;
        }
    }

    public static class HudModel
    {
        public static HudSnapshot Build(HeroVitality hero, string nextObjective,
            InputDeviceTracker device, ThreatCue threat, bool hintAvailable, PlayerPreferences prefs)
        {
            float cur = hero?.Current ?? 0f;
            float max = hero?.Max ?? 0f;
            string glyphs = device?.GlyphSet ?? "glyphs_touch";
            bool subtitles = prefs == null || prefs.Subtitles;
            return new HudSnapshot(cur, max, nextObjective ?? "", glyphs, threat, hintAvailable, subtitles);
        }
    }
}
