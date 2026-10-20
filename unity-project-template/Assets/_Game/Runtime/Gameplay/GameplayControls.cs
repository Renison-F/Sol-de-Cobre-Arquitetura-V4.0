using UnityEngine;
using UnityEngine.InputSystem;

namespace SolDeCobre.Gameplay
{
    // Fonte única do mapa de ações: teclado, gamepad e touch mapeados por AÇÃO
    // (controles-touch.md). O touch é sintetizado pelos OnScreen controls da UI
    // sobre os mesmos caminhos de gamepad, então não há binding de dispositivo na
    // regra de jogo. Reutilizado pelo laboratório (Editor) e pela ponte de runtime.
    public static class GameplayControls
    {
        public const string Map = "Gameplay";
        public const string Move = "Move";
        public const string Jump = "Jump";
        public const string Interact = "Interact";
        public const string Ability = "Ability";
        public const string SwitchHero = "SwitchHero";
        public const string Pause = "Pause";

        // Caminhos que os botões/stick de toque sintetizam (mesma origem do gamepad).
        public const string TouchStickPath = "<Gamepad>/leftStick";
        public const string TouchJumpPath = "<Gamepad>/buttonSouth";
        public const string TouchAbilityPath = "<Gamepad>/buttonWest";
        public const string TouchInteractPath = "<Gamepad>/buttonNorth";
        public const string TouchSwitchPath = "<Gamepad>/rightShoulder";
        public const string TouchPausePath = "<Gamepad>/start";

        public static InputActionAsset Build()
        {
            var asset = ScriptableObject.CreateInstance<InputActionAsset>();
            var map = asset.AddActionMap(Map);

            var move = map.AddAction(Move, InputActionType.Value, expectedControlLayout: "Axis");
            move.AddCompositeBinding("1DAxis").With("Negative", "<Keyboard>/a").With("Positive", "<Keyboard>/d");
            move.AddCompositeBinding("1DAxis").With("Negative", "<Keyboard>/leftArrow").With("Positive", "<Keyboard>/rightArrow");
            move.AddCompositeBinding("1DAxis").With("Negative", "<Gamepad>/dpad/left").With("Positive", "<Gamepad>/dpad/right");
            // Deadzone é aplicada por AxisProcessing na ponte (fonte única e testada),
            // não como processor do asset — evita dupla aplicação em stick + touch.
            move.AddBinding("<Gamepad>/leftStick/x");

            var jump = map.AddAction(Jump, InputActionType.Button);
            jump.AddBinding("<Keyboard>/space");
            jump.AddBinding(TouchJumpPath);

            var interact = map.AddAction(Interact, InputActionType.Button);
            interact.AddBinding("<Keyboard>/e");
            interact.AddBinding(TouchInteractPath);

            var ability = map.AddAction(Ability, InputActionType.Button);
            ability.AddBinding("<Keyboard>/leftShift");
            ability.AddBinding(TouchAbilityPath);

            var switchHero = map.AddAction(SwitchHero, InputActionType.Button);
            switchHero.AddBinding("<Keyboard>/q");
            switchHero.AddBinding(TouchSwitchPath);

            var pause = map.AddAction(Pause, InputActionType.Button);
            pause.AddBinding("<Keyboard>/escape");
            pause.AddBinding(TouchPausePath);

            return asset;
        }
    }
}
