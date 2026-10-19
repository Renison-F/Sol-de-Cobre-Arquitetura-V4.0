using System;
namespace SolDeCobre.Gameplay
{
    // Vida do herói (padrão 5). Dano 1 por acerto, ou 0,5 acumulado com assistência.
    // Morte restaura checkpoint (runtime); aqui só a contabilidade pura.
    public sealed class HeroVitality
    {
        public readonly float Max;
        float current;

        public HeroVitality(float max = 5f) { Max = Math.Max(1f, max); current = Max; }

        public float Current => current;
        public bool IsDead => current <= 0f;

        // Aplica dano; devolve true se o herói morreu com este acerto.
        public bool TakeDamage(float amount)
        {
            if (IsDead || amount <= 0f) return IsDead;
            current = Math.Max(0f, current - amount);
            return IsDead;
        }

        public void Reset() => current = Max;
        public void Restore(float value) => current = Math.Clamp(value, 0f, Max);
    }
}
