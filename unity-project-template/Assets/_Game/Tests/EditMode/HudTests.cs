using NUnit.Framework;
using SolDeCobre.Gameplay;
using SolDeCobre.Hud;
using SolDeCobre.Progression;

namespace SolDeCobre.Tests
{
    public sealed class ThreatPresentationTests
    {
        [Test] public void ReducedFlashesKeepsShapeAndSoundWithoutFlash()
        {
            var cue = ThreatPresentation.For(ThreatLevel.Active, reducedFlashes: true);
            Assert.IsTrue(cue.Active);
            Assert.IsFalse(cue.Flash);          // flash intenso nunca é necessário
            Assert.IsTrue(cue.PlaySound);       // ameaça comunicada por som
            Assert.AreEqual("strike", cue.ShapeId); // e por forma (não só cor)
        }
        [Test] public void FlashAllowedOnlyWhenNotReduced()
        {
            Assert.IsTrue(ThreatPresentation.For(ThreatLevel.Warning, false).Flash);
            Assert.IsFalse(ThreatPresentation.For(ThreatLevel.Warning, true).Flash);
        }
        [Test] public void PhaseMappingMatchesTelegraphAndActive()
        {
            Assert.AreEqual(ThreatLevel.Warning, ThreatPresentation.FromEnemyPhase(EnemyPhase.Prepare));
            Assert.AreEqual(ThreatLevel.Active, ThreatPresentation.FromEnemyPhase(EnemyPhase.Active));
            Assert.AreEqual(ThreatLevel.None, ThreatPresentation.FromEnemyPhase(EnemyPhase.Idle));
            Assert.AreEqual(ThreatLevel.Warning, ThreatPresentation.FromBossPhase(BossPhase.Telegraph));
            Assert.AreEqual(ThreatLevel.Active, ThreatPresentation.FromBossPhase(BossPhase.Active));
            Assert.AreEqual(ThreatLevel.None, ThreatPresentation.FromBossPhase(BossPhase.Exposed));
        }
    }

    public sealed class PlayerPreferencesTests
    {
        [Test] public void ClampKeepsValuesInRange()
        {
            var p = new PlayerPreferences { HudScale = 5f, HudOpacity = 0.1f, SubtitleScale = 9f };
            p.Clamp();
            Assert.AreEqual(1.4f, p.HudScale, 1e-4f);
            Assert.AreEqual(0.5f, p.HudOpacity, 1e-4f);
            Assert.AreEqual(2f, p.SubtitleScale, 1e-4f);
        }
        [Test] public void RoundTripsThePersistedSaveOptions()
        {
            var p = new PlayerPreferences { Assist = true, Subtitles = false, ReducedFlashes = false };
            var opt = p.ToSaveOptions();
            Assert.IsTrue(opt.assist);
            Assert.IsFalse(opt.subtitles);
            Assert.IsFalse(opt.reduced_flashes);
            var q = new PlayerPreferences();
            q.ApplySaveOptions(opt);
            Assert.IsTrue(q.Assist);
            Assert.IsFalse(q.Subtitles);
            Assert.IsFalse(q.ReducedFlashes);
        }
    }

    public sealed class InputDeviceTrackerTests
    {
        [Test] public void TracksLastDeviceAndReportsChange()
        {
            var t = new InputDeviceTracker();
            Assert.IsTrue(t.Use(InputDevice.Gamepad));
            Assert.AreEqual("glyphs_gamepad", t.GlyphSet);
            Assert.IsFalse(t.Use(InputDevice.Gamepad)); // sem mudança
            Assert.IsTrue(t.Use(InputDevice.Keyboard));
            Assert.AreEqual("glyphs_keyboard", t.GlyphSet);
        }
    }

    public sealed class AudioCueBusTests
    {
        [Test] public void DamageWarningOutranksDialogueAndAmbience()
        {
            var bus = new AudioCueBus();
            bus.Raise(new AudioCue("amb", CuePriority.Ambience, AudioBus.Sfx));
            bus.Raise(new AudioCue("talk", CuePriority.Dialogue, AudioBus.Voice));
            bus.Raise(new AudioCue("warn", CuePriority.DamageWarning, AudioBus.Sfx));
            Assert.IsTrue(bus.TryPickHighest(out var top));
            Assert.AreEqual("warn", top.Id);
        }
        [Test] public void WarningStaysAudibleWithMusicMuted()
        {
            var warn = new AudioCue("warn", CuePriority.DamageWarning, AudioBus.Sfx);
            Assert.IsTrue(AudioCueBus.AudibleWhenMusicMuted(warn));
            var song = new AudioCue("theme", CuePriority.Detail, AudioBus.Music);
            Assert.IsFalse(AudioCueBus.AudibleWhenMusicMuted(song));
        }
    }

    public sealed class HudModelTests
    {
        [Test] public void BuildComposesStateForTheView()
        {
            var hero = new HeroVitality(5f); hero.TakeDamage(1f);
            var device = new InputDeviceTracker(); device.Use(InputDevice.Gamepad);
            var threat = ThreatPresentation.For(ThreatLevel.Warning, reducedFlashes: true);
            var prefs = new PlayerPreferences { Subtitles = true };
            var snap = HudModel.Build(hero, "Examinar o poço", device, threat, hintAvailable: true, prefs);
            Assert.AreEqual(4f, snap.HeartsCurrent, 1e-4f);
            Assert.AreEqual(5f, snap.HeartsMax, 1e-4f);
            Assert.AreEqual("Examinar o poço", snap.NextObjective);
            Assert.AreEqual("glyphs_gamepad", snap.GlyphSet);
            Assert.IsTrue(snap.Threat.Active);
            Assert.IsFalse(snap.Threat.Flash);
            Assert.IsTrue(snap.HintAvailable);
            Assert.IsTrue(snap.Subtitles);
        }
    }
}
