using UnityEngine;
using UnityEngine.UI;
using SolDeCobre.Hud;
namespace SolDeCobre.UI
{
    // View da HUD: só desenha o HudSnapshot (fonte da verdade é o jogo). Vida em pips,
    // objetivo curto, ameaça por FORMA sempre e flash só como reforço opcional. Montar
    // o canvas no editor (Image com fillAmount para os corações). Legível a 720p/safe area.
    public sealed class HudController : MonoBehaviour
    {
        [SerializeField] Image[] hearts;           // pips (Image Filled)
        [SerializeField] Text objectiveLabel;      // próxima ação concreta
        [SerializeField] GameObject threatShape;   // forma de ameaça (não depende de cor)
        [SerializeField] Graphic threatFlash;      // reforço opcional
        [SerializeField] CanvasGroup threatGroup;

        public void SetSnapshot(HudSnapshot s)
        {
            if (hearts != null)
                for (int i = 0; i < hearts.Length; i++)
                    if (hearts[i] != null) hearts[i].fillAmount = Mathf.Clamp01(s.HeartsCurrent - i);

            if (objectiveLabel != null) objectiveLabel.text = s.NextObjective;

            if (threatShape != null) threatShape.SetActive(s.Threat.Active);
            if (threatFlash != null) threatFlash.enabled = s.Threat.Active && s.Threat.Flash;
            if (threatGroup != null)
                threatGroup.alpha = s.Threat.Active ? Mathf.Clamp01(0.4f + 0.6f * s.Threat.Intensity) : 0f;
        }
    }
}
