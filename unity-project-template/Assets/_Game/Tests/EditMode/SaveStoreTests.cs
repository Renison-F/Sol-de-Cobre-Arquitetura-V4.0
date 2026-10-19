using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using SolDeCobre.Progression;

namespace SolDeCobre.Tests
{
    // Codec engine-free (formato por linhas) para exercitar o SaveStore em EditMode
    // e no harness .NET, sem depender de JsonUtility. V1 legado é escrito via helper.
    sealed class FakeCodec : ISaveCodec
    {
        public string Serialize(SaveDataV3 d) =>
            "V3\nslot=" + d.slot + "\nrevision=" + d.revision + "\nchapter=" + d.chapter +
            "\ncheckpoint=" + d.checkpoint + "\nhero=" + d.hero +
            "\nflags=" + string.Join("|", d.flags) + "\natlas=" + string.Join("|", d.atlas) +
            "\nwritten_at=" + d.written_at +
            "\noptions=" + B(d.options.assist) + "|" + B(d.options.subtitles) + "|" + B(d.options.reduced_flashes);

        public string SerializeV1(SaveSnapshot s) =>
            "V1\nchapter=" + s.chapterId + "\ncheckpoint=" + s.checkpointId +
            "\nhero=" + s.heroId + "\nflags=" + string.Join("|", s.questFlags);

        static string B(bool b) => b ? "1" : "0";

        public int ProbeVersion(string text)
        {
            if (string.IsNullOrEmpty(text)) return -1;
            string first = text.Split('\n')[0].Trim();
            if (first.Length >= 2 && first[0] == 'V' && int.TryParse(first.Substring(1), out int v)) return v;
            return -1;
        }

        static Dictionary<string, string> Fields(string text)
        {
            var map = new Dictionary<string, string>();
            foreach (var line in text.Split('\n'))
            {
                int eq = line.IndexOf('=');
                if (eq > 0) map[line.Substring(0, eq)] = line.Substring(eq + 1);
            }
            return map;
        }
        static string[] Split(string v) =>
            string.IsNullOrEmpty(v) ? Array.Empty<string>() : v.Split('|');

        public bool TryDeserializeV3(string text, out SaveDataV3 data)
        {
            data = null;
            var f = Fields(text);
            if (!f.TryGetValue("slot", out var slot) || !int.TryParse(slot, out int slotN)) return false;
            if (!f.TryGetValue("revision", out var rev) || !int.TryParse(rev, out int revN)) return false;
            if (!f.TryGetValue("chapter", out var chapter) || !f.TryGetValue("hero", out var hero)) return false;
            long.TryParse(f.TryGetValue("written_at", out var w) ? w : "0", out long written);
            var opt = Split(f.TryGetValue("options", out var o) ? o : "0|1|1");
            data = new SaveDataV3
            {
                slot = slotN, revision = revN, chapter = chapter, hero = hero,
                checkpoint = f.TryGetValue("checkpoint", out var cp) ? cp : "",
                flags = Split(f.TryGetValue("flags", out var fl) ? fl : ""),
                atlas = Split(f.TryGetValue("atlas", out var at) ? at : ""),
                written_at = written,
                options = new SaveOptions
                {
                    assist = opt.Length > 0 && opt[0] == "1",
                    subtitles = opt.Length < 2 || opt[1] == "1",
                    reduced_flashes = opt.Length < 3 || opt[2] == "1"
                }
            };
            return true;
        }

        public bool TryDeserializeV1(string text, out SaveSnapshot data)
        {
            data = null;
            var f = Fields(text);
            if (!f.TryGetValue("chapter", out var chapter) || !f.TryGetValue("hero", out var hero)) return false;
            data = new SaveSnapshot
            {
                schemaVersion = 1, chapterId = chapter, heroId = hero,
                checkpointId = f.TryGetValue("checkpoint", out var cp) ? cp : "",
                questFlags = Split(f.TryGetValue("flags", out var fl) ? fl : "")
            };
            return true;
        }
    }

    public sealed class SaveStoreTests
    {
        string dir;
        FakeCodec codec;
        SaveStore store;

        [SetUp] public void SetUp()
        {
            dir = Path.Combine(Path.GetTempPath(), "sc-save-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            codec = new FakeCodec();
            store = new SaveStore(dir, codec, FlagCatalog.Default(), () => 1000);
        }
        [TearDown] public void TearDown()
        {
            try { if (Directory.Exists(dir)) Directory.Delete(dir, true); } catch { }
        }

        static SaveDataV3 Data(int slot, int revision, string chapter = "LVL-01") => new SaveDataV3
        {
            schema_version = 3, slot = slot, revision = revision, chapter = chapter,
            checkpoint = "CP-01-01", hero = "mara",
            flags = new[] { "POCO_EXAMINADO" }, atlas = Array.Empty<string>(),
            written_at = 500, options = new SaveOptions()
        };

        [Test] public void MissingSlotReportsMissing()
        {
            Assert.AreEqual(SaveLoadStatus.Missing, store.Read(3).Status);
        }
        [Test] public void WriteThenReadRoundTrips()
        {
            Assert.IsTrue(store.Write(Data(1, 1)));
            var r = store.Read(1);
            Assert.AreEqual(SaveLoadStatus.Loaded, r.Status);
            Assert.AreEqual("LVL-01", r.Data.chapter);
            Assert.AreEqual(1, r.Data.revision);
        }
        [Test] public void OverwriteKeepsBackupAndRecoversAfterCorruption()
        {
            Assert.IsTrue(store.Write(Data(1, 1)));
            Assert.IsTrue(store.Write(Data(1, 2)));
            string path = Path.Combine(dir, "sol-cobre-slot-1.json");
            Assert.IsTrue(File.Exists(path + ".bak"), "backup deve existir após promoção");
            // Corrompe o principal; o backup guarda a revisão 1.
            File.WriteAllText(path, "lixo sem cabeçalho");
            var r = store.Read(1);
            Assert.AreEqual(SaveLoadStatus.RecoveredBackup, r.Status);
            Assert.AreEqual(1, r.Data.revision);
            Assert.IsTrue(File.Exists(path), "arquivo corrompido é preservado, não apagado");
        }
        [Test] public void CorruptPrimaryAndBackupPreservesWithoutReset()
        {
            Assert.IsTrue(store.Write(Data(1, 1)));
            Assert.IsTrue(store.Write(Data(1, 2)));
            string path = Path.Combine(dir, "sol-cobre-slot-1.json");
            File.WriteAllText(path, "lixo");
            File.WriteAllText(path + ".bak", "lixo tambem");
            var r = store.Read(1);
            Assert.AreEqual(SaveLoadStatus.CorruptPreserved, r.Status);
            Assert.IsNull(r.Data);
        }
        [Test] public void FutureVersionIsPreservedNotOverwritten()
        {
            string path = Path.Combine(dir, "sol-cobre-slot-2.json");
            File.WriteAllText(path, "V4\nslot=2\nchapter=LVL-01\nhero=mara");
            Assert.AreEqual(SaveLoadStatus.UnsupportedVersion, store.Read(2).Status);
        }
        [Test] public void LegacyV1OnDiskIsMigratedOnRead()
        {
            string path = Path.Combine(dir, "sol-cobre-slot-1.json");
            var v1 = new SaveSnapshot
            {
                schemaVersion = 1, chapterId = "LVL-05", heroId = "ze", checkpointId = "CP-05-01",
                questFlags = new[] { "POCO_EXAMINADO", "FLAG_FALSA" }
            };
            File.WriteAllText(path, codec.SerializeV1(v1));
            var r = store.Read(1);
            Assert.AreEqual(SaveLoadStatus.Migrated, r.Status);
            Assert.AreEqual("LVL-05", r.Data.chapter);
            Assert.AreEqual("ze", r.Data.hero);
            CollectionAssert.AreEquivalent(new[] { "POCO_EXAMINADO" }, r.Data.flags);
            CollectionAssert.Contains(r.Migration.UnknownFlags, "FLAG_FALSA");
        }
        [Test] public void SlotsAreIndependent()
        {
            Assert.IsTrue(store.Write(Data(1, 1, "LVL-01")));
            Assert.IsTrue(store.Write(Data(2, 1, "LVL-02")));
            Assert.AreEqual("LVL-01", store.Read(1).Data.chapter);
            Assert.AreEqual("LVL-02", store.Read(2).Data.chapter);
            Assert.AreEqual(SaveLoadStatus.Missing, store.Read(3).Status);
        }
        [Test] public void InvalidDataIsRejectedOnWrite()
        {
            var bad = Data(1, 1); bad.hero = "luna";
            Assert.IsFalse(store.Write(bad));
            Assert.AreEqual(SaveLoadStatus.Missing, store.Read(1).Status);
        }
        [Test] public void OversizedPayloadIsRejected()
        {
            var big = new SaveStore(dir, new OversizedCodec(), FlagCatalog.Default());
            Assert.IsFalse(big.Write(Data(1, 1)));
        }

        sealed class OversizedCodec : ISaveCodec
        {
            public string Serialize(SaveDataV3 d) => new string('x', 2 * 1024 * 1024);
            public int ProbeVersion(string text) => -1;
            public bool TryDeserializeV3(string text, out SaveDataV3 data) { data = null; return false; }
            public bool TryDeserializeV1(string text, out SaveSnapshot data) { data = null; return false; }
        }
    }
}
