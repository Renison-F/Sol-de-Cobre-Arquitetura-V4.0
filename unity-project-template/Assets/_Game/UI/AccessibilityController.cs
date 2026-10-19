using UnityEngine;
using SolDeCobre.Hud;
using SolDeCobre.Progression;
namespace SolDeCobre.UI
{
    // Aplica as preferências de acessibilidade à HUD de toque e ao áudio, e projeta
    // as três opções persistidas no save (assist/subtitles/reduced_flashes). As demais
    // (escala/opacidade/canhoto/legendas/volumes) ficam no perfil local. Disponível
    // antes de jogar; "restaurar defaults" com preview.
    public sealed class AccessibilityController : MonoBehaviour
    {
        [SerializeField] TouchHud touchHud;
        [SerializeField] AudioDirector audioDirector;

        readonly PlayerPreferences prefs = new PlayerPreferences();
        public PlayerPreferences Preferences => prefs;
        public bool ReducedFlashes => prefs.ReducedFlashes;

        void Start() => Apply();

        public void Apply()
        {
            prefs.Clamp();
            if (touchHud != null)
            {
                touchHud.SetScale(prefs.HudScale);
                touchHud.SetOpacity(prefs.HudOpacity);
                touchHud.SetLeftHanded(prefs.Hand == Handedness.Left);
            }
            if (audioDirector != null)
            {
                audioDirector.SetBusVolume(AudioBus.Music, prefs.MusicVolume);
                audioDirector.SetBusVolume(AudioBus.Sfx, prefs.SfxVolume);
                audioDirector.SetBusVolume(AudioBus.Voice, prefs.VoiceVolume);
            }
        }

        public void RestoreDefaults() { prefs.RestoreDefaults(); Apply(); }

        public SaveOptions ToSaveOptions() => prefs.ToSaveOptions();
        public void LoadFrom(SaveOptions options) { prefs.ApplySaveOptions(options); Apply(); }
    }
}
