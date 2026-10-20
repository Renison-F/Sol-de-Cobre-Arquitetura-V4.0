using System;
using SolDeCobre.Progression;
namespace SolDeCobre.Hud
{
    public enum Handedness { Right, Left }

    // Preferências por perfil (opções locais). As três que o save guarda (assist,
    // subtitles, reduced_flashes) viajam no SaveOptions; as demais ficam em arquivo
    // de preferências separado (telas-e-fluxos.md: "tudo persiste por perfil").
    // Valores com clamp e defaults documentados; acessibilidade disponível antes de jogar.
    [System.Serializable]
    public sealed class PlayerPreferences
    {
        public bool Assist;
        public bool Subtitles = true;
        public bool ReducedFlashes = true;
        public bool ReducedShake = true;
        public Handedness Hand = Handedness.Right;
        public float HudScale = 1f;       // 0.8–1.4 (controles-touch.md)
        public float HudOpacity = 1f;     // 0.5–1.0
        public float SubtitleScale = 1f;  // 0.8–2.0 (zoom de legendas)
        public float MusicVolume = 1f, SfxVolume = 1f, VoiceVolume = 1f; // mix separado

        public void Clamp()
        {
            HudScale = Math.Clamp(HudScale, 0.8f, 1.4f);
            HudOpacity = Math.Clamp(HudOpacity, 0.5f, 1f);
            SubtitleScale = Math.Clamp(SubtitleScale, 0.8f, 2f);
            MusicVolume = Math.Clamp(MusicVolume, 0f, 1f);
            SfxVolume = Math.Clamp(SfxVolume, 0f, 1f);
            VoiceVolume = Math.Clamp(VoiceVolume, 0f, 1f);
        }

        // Projeta as três opções persistidas no contrato de save V3.
        public SaveOptions ToSaveOptions() =>
            new SaveOptions { assist = Assist, subtitles = Subtitles, reduced_flashes = ReducedFlashes };

        public void ApplySaveOptions(SaveOptions options)
        {
            if (options == null) return;
            Assist = options.assist;
            Subtitles = options.subtitles;
            ReducedFlashes = options.reduced_flashes;
        }

        public PlayerPreferences RestoreDefaults()
        {
            Assist = false; Subtitles = true; ReducedFlashes = true; ReducedShake = true;
            Hand = Handedness.Right;
            HudScale = 1f; HudOpacity = 1f; SubtitleScale = 1f;
            MusicVolume = SfxVolume = VoiceVolume = 1f;
            return this;
        }
    }
}
