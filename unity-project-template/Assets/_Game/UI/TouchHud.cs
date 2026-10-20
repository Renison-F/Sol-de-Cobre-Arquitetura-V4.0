using UnityEngine;

namespace SolDeCobre.UI
{
    // Configuração do HUD de toque: opacidade 50–100%, escala 80–140%, canhoto e
    // respeito às safe areas reais do SO (controles-touch.md). Reposicionar com
    // botão restaurar volta aos clusters padrão. Orientação landscape assumida.
    public sealed class TouchHud : MonoBehaviour
    {
        [SerializeField] CanvasGroup group;
        [SerializeField] RectTransform safeAreaPanel;
        [SerializeField] RectTransform moveCluster;   // direcional (padrão: esquerda)
        [SerializeField] RectTransform actionCluster; // salto/habilidade (padrão: direita)
        [SerializeField, Range(.5f, 1f)] float opacity = 1f;
        [SerializeField, Range(.8f, 1.4f)] float scale = 1f;
        [SerializeField] bool leftHanded;

        Vector2 moveAnchorMin, moveAnchorMax, movePivot, moveAnchoredPos;
        Vector2 actionAnchorMin, actionAnchorMax, actionPivot, actionAnchoredPos;
        bool captured;
        Rect lastSafeArea;

        void Awake() { CaptureDefaults(); ApplyAll(); }
        void OnEnable() { ApplyAll(); }

        void Update()
        {
            // Reaplica safe area quando a área útil muda (rotação bloqueada, notch, etc.).
            if (safeAreaPanel != null && Screen.safeArea != lastSafeArea) ApplySafeArea();
        }

        void CaptureDefaults()
        {
            if (captured) return;
            if (moveCluster != null)
            {
                moveAnchorMin = moveCluster.anchorMin; moveAnchorMax = moveCluster.anchorMax;
                movePivot = moveCluster.pivot; moveAnchoredPos = moveCluster.anchoredPosition;
            }
            if (actionCluster != null)
            {
                actionAnchorMin = actionCluster.anchorMin; actionAnchorMax = actionCluster.anchorMax;
                actionPivot = actionCluster.pivot; actionAnchoredPos = actionCluster.anchoredPosition;
            }
            captured = true;
        }

        public void ApplyAll()
        {
            CaptureDefaults();
            SetOpacity(opacity);
            SetScale(scale);
            SetLeftHanded(leftHanded);
            ApplySafeArea();
        }

        public void SetOpacity(float value)
        {
            opacity = Mathf.Clamp(value, .5f, 1f);
            if (group != null) group.alpha = opacity;
        }

        public void SetScale(float value)
        {
            scale = Mathf.Clamp(value, .8f, 1.4f);
            if (moveCluster != null) moveCluster.localScale = Vector3.one * scale;
            if (actionCluster != null) actionCluster.localScale = Vector3.one * scale;
        }

        // Canhoto: espelha os clusters trocando as âncoras horizontais (0<->1).
        public void SetLeftHanded(bool value)
        {
            leftHanded = value;
            if (moveCluster != null) ApplyAnchors(moveCluster, moveAnchorMin, moveAnchorMax, movePivot, moveAnchoredPos, value);
            if (actionCluster != null) ApplyAnchors(actionCluster, actionAnchorMin, actionAnchorMax, actionPivot, actionAnchoredPos, value);
        }

        public void RestoreLayout()
        {
            leftHanded = false;
            SetLeftHanded(false);
            SetScale(1f);
            SetOpacity(1f);
        }

        public void ApplySafeArea()
        {
            if (safeAreaPanel == null) return;
            Rect safe = Screen.safeArea;
            lastSafeArea = safe;
            float w = Screen.width, h = Screen.height;
            if (w <= 0f || h <= 0f) return;
            Vector2 min = new Vector2(safe.xMin / w, safe.yMin / h);
            Vector2 max = new Vector2(safe.xMax / w, safe.yMax / h);
            safeAreaPanel.anchorMin = min;
            safeAreaPanel.anchorMax = max;
            safeAreaPanel.offsetMin = Vector2.zero;
            safeAreaPanel.offsetMax = Vector2.zero;
        }

        static void ApplyAnchors(RectTransform rt, Vector2 min, Vector2 max, Vector2 pivot, Vector2 anchoredPos, bool mirror)
        {
            if (!mirror)
            {
                rt.anchorMin = min; rt.anchorMax = max; rt.pivot = pivot;
                rt.anchoredPosition = anchoredPos;
            }
            else
            {
                rt.anchorMin = new Vector2(1f - max.x, min.y);
                rt.anchorMax = new Vector2(1f - min.x, max.y);
                rt.pivot = new Vector2(1f - pivot.x, pivot.y);
                rt.anchoredPosition = new Vector2(-anchoredPos.x, anchoredPos.y);
            }
        }
    }
}
