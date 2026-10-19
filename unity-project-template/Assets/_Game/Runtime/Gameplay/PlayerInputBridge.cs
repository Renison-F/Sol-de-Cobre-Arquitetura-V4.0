using UnityEngine;
using UnityEngine.InputSystem;

namespace SolDeCobre.Gameplay
{
    // Owns a clone: disabling this actor never disables another actor's actions.
    [RequireComponent(typeof(PlayerMotor2_5D))]
    public sealed class PlayerInputBridge : MonoBehaviour
    {
        [SerializeField] InputActionAsset actionTemplate;
        InputActionAsset owned;
        InputAction move, jump;
        PlayerMotor2_5D motor;
        bool focused = true, paused;

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
            move = owned.FindAction("Gameplay/Move", true);
            jump = owned.FindAction("Gameplay/Jump", true);
            move.performed += Move;
            move.canceled += Move;
            jump.performed += Jump;
            jump.canceled += Release;
        }

        void OnEnable() => Sync();
        void OnDisable() { if (owned != null) owned.Disable(); if (motor != null) motor.ClearInput(); }
        void OnApplicationFocus(bool value) { focused = value; Sync(); }
        void OnApplicationPause(bool value) { paused = value; Sync(); }
        void Sync()
        {
            if (owned == null) return;
            if (isActiveAndEnabled && focused && !paused) owned.Enable();
            else { owned.Disable(); if (motor != null) motor.ClearInput(); }
        }
        void Move(InputAction.CallbackContext ctx) => motor.SetHorizontal(ctx.ReadValue<float>());
        void Jump(InputAction.CallbackContext ctx) => motor.QueueJump();
        void Release(InputAction.CallbackContext ctx) => motor.ReleaseJump();
        void OnDestroy()
        {
            if (owned == null) return;
            move.performed -= Move; move.canceled -= Move;
            jump.performed -= Jump; jump.canceled -= Release;
            owned.Disable();
            Destroy(owned);
        }
    }
}
