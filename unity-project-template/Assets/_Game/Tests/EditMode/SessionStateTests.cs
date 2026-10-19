using NUnit.Framework;
using SolDeCobre.Progression;

namespace SolDeCobre.Tests
{
    public sealed class SessionStateTests
    {
        [Test] public void RoundTripsThroughSaveContract()
        {
            var s = new SessionState { Slot = 2, Revision = 3, Chapter = "LVL-04", Checkpoint = "CP-04-02", HeroId = "ze" };
            s.GrantFlag("POCO_EXAMINADO"); s.GrantFlag("LACO_APRENDIDO");
            s.GrantAtlas("ATL-04-01");
            s.Options.assist = true;

            var save = s.ToSave(writtenAt: 777);
            Assert.IsTrue(SaveValidationV3.IsValid(save, out _));

            var back = SessionState.FromSave(save);
            Assert.AreEqual(2, back.Slot);
            Assert.AreEqual(3, back.Revision);
            Assert.AreEqual("LVL-04", back.Chapter);
            Assert.AreEqual("CP-04-02", back.Checkpoint);
            Assert.AreEqual("ze", back.HeroId);
            CollectionAssert.AreEquivalent(new[] { "POCO_EXAMINADO", "LACO_APRENDIDO" }, back.Flags);
            CollectionAssert.AreEquivalent(new[] { "ATL-04-01" }, back.Atlas);
            Assert.IsTrue(back.Options.assist);
        }
        [Test] public void FlagsAndAtlasAreUnique()
        {
            var s = new SessionState();
            Assert.IsTrue(s.GrantFlag("A"));
            Assert.IsFalse(s.GrantFlag("A"));
            Assert.IsTrue(s.GrantAtlas("ATL-01-01"));
            Assert.IsFalse(s.GrantAtlas("ATL-01-01"));
            Assert.AreEqual(1, s.Flags.Count);
        }
        [Test] public void RevisionGrowsPerCheckpoint()
        {
            var s = new SessionState { Revision = 1 };
            Assert.AreEqual(2, s.NextRevision());
            Assert.AreEqual(3, s.NextRevision());
        }
        [Test] public void SwitchHeroRejectsUnknown()
        {
            var s = new SessionState();
            Assert.IsTrue(s.SwitchHero("ze"));
            Assert.IsFalse(s.SwitchHero("luna"));
            Assert.AreEqual("ze", s.HeroId);
        }
    }
}
