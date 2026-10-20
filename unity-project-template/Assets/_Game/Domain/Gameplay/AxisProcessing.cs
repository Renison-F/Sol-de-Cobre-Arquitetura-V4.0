using System;
namespace SolDeCobre.Gameplay
{
    // Regra de movimento pura: não depende de código de tecla nem de engine.
    // Zona morta radial 1D para o direcional virtual (controles-touch.md: dz 0,15).
    public static class AxisProcessing
    {
        public static float ApplyDeadzone(float raw, float deadzone)
        {
            if (float.IsNaN(raw)) return 0f;
            float value = Math.Clamp(raw, -1f, 1f);
            float dz = Math.Clamp(deadzone, 0f, 0.999f);
            float magnitude = Math.Abs(value);
            if (magnitude <= dz) return 0f;
            // Reescala [dz..1] para [0..1] preservando o sinal; sem salto no limiar.
            float scaled = (magnitude - dz) / (1f - dz);
            return value < 0f ? -scaled : scaled;
        }
    }
}
