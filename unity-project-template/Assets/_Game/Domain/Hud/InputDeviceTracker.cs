namespace SolDeCobre.Hud
{
    public enum InputDevice { Keyboard, Gamepad, Touch }

    // Rastreia o dispositivo usado por último para a UI mostrar o glifo certo
    // (telas-e-fluxos.md: "UI navega foco e mostra glifo do dispositivo usado").
    public sealed class InputDeviceTracker
    {
        public InputDevice Current { get; private set; } = InputDevice.Touch;

        // true se mudou (a UI deve trocar os glifos).
        public bool Use(InputDevice device)
        {
            if (device == Current) return false;
            Current = device;
            return true;
        }

        public string GlyphSet => Current switch
        {
            InputDevice.Keyboard => "glyphs_keyboard",
            InputDevice.Gamepad => "glyphs_gamepad",
            _ => "glyphs_touch"
        };
    }
}
