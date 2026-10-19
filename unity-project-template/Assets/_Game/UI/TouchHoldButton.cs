using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

namespace SolDeCobre.UI
{
    // Botão de toque que sintetiza um controle do Input System. Solta a ação ao
    // levantar o dedo OU ao arrastar para fora do botão (controles-touch.md:
    // "arrastar dedo para fora do botão não deixa ação permanentemente ativa").
    // Cada botão tem seu próprio pointer, então habilidade não rouba o salto.
    public sealed class TouchHoldButton : OnScreenControl,
        IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        [InputControl(layout = "Button")]
        [SerializeField] string controlPath;
        int holdingPointer = -1;

        protected override string controlPathInternal
        {
            get => controlPath;
            set => controlPath = value;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            holdingPointer = eventData.pointerId;
            SendValueToControl(1f);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.pointerId != holdingPointer) return;
            Release();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.pointerId != holdingPointer) return;
            Release();
        }

        void Release()
        {
            holdingPointer = -1;
            SendValueToControl(0f);
        }
    }
}
