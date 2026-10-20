using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SolDeCobre.Gameplay
{
    // Dono de um clone: desabilitar este ator nunca desabilita as ações de outro.
    // Coleta input e fornece snapshot ao motor; eventos de ação para HUD/habilidade.
    // Perda de foco, pausa do SO e cancelamento de pointer zeram eixos e botões presos
    // (controles-touch.md). Um botão de habilidade solto por perda de foco dispara
    // AbilityReleased, para não deixar habilidade/corda ativa.
    [RequireComponent(typeof(PlayerMotor2_5D))]
    public sealed class PlayerInputBridge : MonoBehaviour
    {
        [SerializeField] InputActionAsset actionTemplate;
        [SerializeField, Range(0f, 0.5f)] float moveDeadzone = 0.15f;

        public event Action Interacted;
        public event Action AbilityPressed;
        public event Action AbilityReleased;
        public event Action HeroSwitchRequested;
        public event Action PauseToggled;

        InputActionAsset owned;
        InputAction move, jump, interact, ability, switchHero, pause;
        PlayerMotor2_5D motor;
        bool focused = true, paused, abilityHeld;

        void Awake()
        {
            motor = GetComponent<PlayerMotor2_5D>();
            if (actionTemplate == null)
            {
                Debug.LogError("PlayerInputBridge requires an action template.", this);
                enabled = false;
                return;
            }
            owned = Instantiate(actionTemplate);
            string m = GameplayControls.Map + "/";
            move = owned.FindAction(m + GameplayControls.Move, true);
            jump = owned.FindAction(m + GameplayControls.Jump, true);
            interact = owned.FindAction(m + GameplayControls.Interact, true);
            ability = owned.FindAction(m + GameplayControls.Ability, true);
            switchHero = owned.FindAction(m + GameplayControls.SwitchHero, true);
            pause = owned.FindAction(m + GameplayControls.Pause, true);

            move.performed += OnMove; move.canceled += OnMove;
            jump.performed += OnJump; jump.canceled += OnJumpRelease;
            ability.performed += OnAbility; ability.canceled += OnAbilityRelease;
            interact.performed += OnInteract;
            switchHero.performed += OnSwitch;
            pause.performed += OnPause;
        }

        void OnEnable() => Sync();
        void OnDisable() { Disable(); }
        void OnApplicationFocus(bool value) { focused = value; Sync(); }
        void OnApplicationPause(bool value) { paused = value; Sync(); }

        void Sync()
        {
            if (owned == null) return;
            if (isActiveAndEnabled && focused && !paused) owned.Enable();
            else Disable();
        }

        // Estado neutro garantido: nenhum eixo ou botão fica preso ao perder foco/pausar.
        void Disable()
        {
            if (owned != null) owned.Disable();
            if (motor != null) motor.ClearInput();
            if (abilityHeld) { abilityHeld = false; AbilityReleased?.Invoke(); }
        }

        void OnMove(InputAction.CallbackContext ctx) =>
            motor.SetHorizontal(AxisProcessing.ApplyDeadzone(ctx.ReadValue<float>(), moveDeadzone));
        void OnJump(InputAction.CallbackContext ctx) => motor.QueueJump();
        void OnJumpRelease(InputAction.CallbackContext ctx) => motor.ReleaseJump();
        void OnInteract(InputAction.CallbackContext ctx) => Interacted?.Invoke();
        void OnAbility(InputAction.CallbackContext ctx) { abilityHeld = true; AbilityPressed?.Invoke(); }
        void OnAbilityRelease(InputAction.CallbackContext ctx)
        {
            if (!abilityHeld) return;
            abilityHeld = false; AbilityReleased?.Invoke();
        }
        void OnSwitch(InputAction.CallbackContext ctx) => HeroSwitchRequested?.Invoke();
        void OnPause(InputAction.CallbackContext ctx) => PauseToggled?.Invoke();

        void OnDestroy()
        {
            if (owned == null) return;
            move.performed -= OnMove; move.canceled -= OnMove;
            jump.performed -= OnJump; jump.canceled -= OnJumpRelease;
            ability.performed -= OnAbility; ability.canceled -= OnAbilityRelease;
            interact.performed -= OnInteract;
            switchHero.performed -= OnSwitch;
            pause.performed -= OnPause;
            owned.Disable();
            Destroy(owned);
        }
    }
}
