using NUnit.Framework;
using SolDeCobre.Progression;

namespace SolDeCobre.Tests
{
    public sealed class SaveValidationV3Tests
    {
        static SaveDataV3 Example() => new SaveDataV3
        {
            schema_version = 3, slot = 1, revision = 1,
            chapter = "LVL-01", checkpoint = "CP-01-01", hero = "mara",
            flags = new string[0], atlas = new string[0], written_at = 1802865600,
            options = new SaveOptions { assist = false, subtitles = true, reduced_flashes = true }
        };

        [Test] public void ContractExampleIsValid()
        {
            Assert.IsTrue(SaveValidationV3.IsValid(Example(), out _));
        }
        [Test] public void RejectsBadChapterSlotHeroAndAtlas()
        {
            var bad = Example(); bad.chapter = "LVL-11";
            Assert.IsFalse(SaveValidationV3.IsValid(bad, out _));
            bad = Example(); bad.slot = 4;
            Assert.IsFalse(SaveValidationV3.IsValid(bad, out _));
            bad = Example(); bad.hero = "luna";
            Assert.IsFalse(SaveValidationV3.IsValid(bad, out _));
            bad = Example(); bad.atlas = new[] { "ATL-01-04" }; // 04 fora de 01-03
            Assert.IsFalse(SaveValidationV3.IsValid(bad, out _));
        }
        [Test] public void RejectsDuplicateFlags()
        {
            var bad = Example(); bad.flags = new[] { "A", "A" };
            Assert.IsFalse(SaveValidationV3.IsValid(bad, out string reason));
            Assert.AreEqual("flag duplicada", reason);
        }
        [Test] public void AcceptsValidAtlasIds()
        {
            var ok = Example(); ok.atlas = new[] { "ATL-01-01", "ATL-10-03" };
            Assert.IsTrue(SaveValidationV3.IsValid(ok, out _));
        }
    }

    public sealed class SaveMigratorTests
    {
        static SaveSnapshot V1(string chapter, string hero, string cp, string[] flags) => new SaveSnapshot
        {
            schemaVersion = 1, chapterId = chapter, checkpointId = cp, heroId = hero,
            questFlags = flags ?? new string[0]
        };

        [Test] public void Case1_ValidV1MapsToEquivalentV3PreservingKnownFlags()
        {
            var v1 = V1("LVL-03", "ze", "CP-03-02",
                new[] { "POCO_EXAMINADO", "LACO_APRENDIDO", "FLAG_INVENTADA" });
            var r = SaveMigrator.FromV1(v1, slot: 2, writtenAt: 123, FlagCatalog.Default());

            Assert.AreEqual(MigrationStatus.Migrated, r.Status);
            Assert.AreEqual(3, r.Data.schema_version);
            Assert.AreEqual(2, r.Data.slot);
            Assert.AreEqual(1, r.Data.revision);
            Assert.AreEqual("LVL-03", r.Data.chapter);
            Assert.AreEqual("ze", r.Data.hero);
            Assert.AreEqual("CP-03-02", r.Data.checkpoint);
            Assert.AreEqual(123, r.Data.written_at);
            CollectionAssert.AreEquivalent(new[] { "POCO_EXAMINADO", "LACO_APRENDIDO" }, r.Data.flags);
            // defaults documentados
            Assert.IsFalse(r.Data.options.assist);
            Assert.IsTrue(r.Data.options.subtitles);
            Assert.IsTrue(r.Data.options.reduced_flashes);
            // desconhecida só no relatório, nunca no save
            CollectionAssert.Contains(r.Report.UnknownFlags, "FLAG_INVENTADA");
            Assert.IsTrue(SaveValidationV3.IsValid(r.Data, out _));
        }
        [Test] public void Case2_UnknownFlagsNeverBecomeProgress()
        {
            var v1 = V1("LVL-01", "mara", "CP-01-01", new[] { "COMPRA_PREMIUM", "ABILITY_HACK" });
            var r = SaveMigrator.FromV1(v1, 1, 0, FlagCatalog.Default());
            Assert.AreEqual(MigrationStatus.Migrated, r.Status);
            Assert.AreEqual(0, r.Data.flags.Length);
            Assert.AreEqual(2, r.Report.UnknownFlags.Count);
        }
        [Test] public void InvalidChapterOrHeroIsCorrupt()
        {
            Assert.AreEqual(MigrationStatus.Corrupt,
                SaveMigrator.FromV1(V1("LVL-99", "mara", "", null), 1, 0, FlagCatalog.Default()).Status);
            Assert.AreEqual(MigrationStatus.Corrupt,
                SaveMigrator.FromV1(V1("LVL-01", "ninguem", "", null), 1, 0, FlagCatalog.Default()).Status);
        }
        [Test] public void FutureVersionIsUnsupported()
        {
            var future = new SaveSnapshot { schemaVersion = 5, chapterId = "LVL-01", heroId = "mara", questFlags = new string[0] };
            Assert.AreEqual(MigrationStatus.Unsupported,
                SaveMigrator.FromV1(future, 1, 0, FlagCatalog.Default()).Status);
        }
        [Test] public void OversizedCheckpointFallsBackToSafeEntryWithWarning()
        {
            var v1 = V1("LVL-02", "mara", new string('x', 70), null);
            var r = SaveMigrator.FromV1(v1, 1, 0, FlagCatalog.Default());
            Assert.AreEqual(MigrationStatus.Migrated, r.Status);
            Assert.AreEqual("", r.Data.checkpoint);
            Assert.IsNotEmpty(r.Report.Warnings);
        }
    }
}
