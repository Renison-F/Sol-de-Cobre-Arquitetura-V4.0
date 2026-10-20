using SolDeCobre.Gameplay;
namespace SolDeCobre.Hud
{
    public enum ThreatLevel { None, Warning, Active }

    // Descritor de ameaça para a HUD/áudio. Regras de acessibilidade (hud-acessibilidade.md,
    // 06-audio-...md): ameaça comunicada por FORMA e SOM, nunca só por cor; flash intenso
    // nunca é necessário para perceber a ameaça (desligado em "reduzir flashes"), mas forma
    // e som permanecem — leitura preservada em perfil gráfico baixo.
    public readonly struct ThreatCue
    {
        public readonly bool Active;
        public readonly string ShapeId; // forma reconhecível (não depende de cor)
        public readonly bool PlaySound; // ameaça sempre audível
        public readonly bool Flash;     // opcional; falso com "reduzir flashes"
        public readonly float Intensity;
        public ThreatCue(bool active, string shapeId, bool playSound, bool flash, float intensity)
        { Active = active; ShapeId = shapeId; PlaySound = playSound; Flash = flash; Intensity = intensity; }
    }

    public static class ThreatPresentation
    {
        public static ThreatCue For(ThreatLevel level, bool reducedFlashes)
        {
            switch (level)
            {
                case ThreatLevel.Warning:
                    // Aviso: forma de alerta + som; flash só como reforço opcional.
                    return new ThreatCue(true, "warn", true, !reducedFlashes, 0.6f);
                case ThreatLevel.Active:
                    // Golpe ativo: forma de impacto + som; flash nunca obrigatório.
                    return new ThreatCue(true, "strike", true, !reducedFlashes, 1f);
                default:
                    return new ThreatCue(false, null, false, false, 0f);
            }
        }

        public static ThreatLevel FromEnemyPhase(EnemyPhase phase) => phase switch
        {
            EnemyPhase.Prepare => ThreatLevel.Warning,
            EnemyPhase.Active => ThreatLevel.Active,
            _ => ThreatLevel.None
        };

        public static ThreatLevel FromBossPhase(BossPhase phase) => phase switch
        {
            BossPhase.Telegraph => ThreatLevel.Warning,
            BossPhase.Active => ThreatLevel.Active,
            _ => ThreatLevel.None
        };
    }
}
