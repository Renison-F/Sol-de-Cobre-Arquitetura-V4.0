using System;
using System.IO;
using System.Text;
namespace SolDeCobre.Progression
{
    public enum SaveLoadStatus { Missing, Loaded, Migrated, RecoveredBackup, CorruptPreserved, UnsupportedVersion }

    public sealed class SaveLoadResult
    {
        public SaveLoadStatus Status;
        public SaveDataV3 Data;
        public MigrationReport Migration; // preenchido quando houve migração 1->3
        public string Reason;
    }

    // Três slots independentes. Gravação atômica (tmp + flush + promoção com backup).
    // Leitura nunca substitui dado corrompido por campanha nova silenciosamente:
    // tenta o backup e, se falhar, preserva o arquivo e devolve CorruptPreserved.
    // Versão futura é preservada, não sobrescrita. Engine-free (System.IO + ISaveCodec).
    public sealed class SaveStore
    {
        const long MaxBytes = 1048576;
        readonly string root;
        readonly ISaveCodec codec;
        readonly FlagCatalog catalog;
        readonly Func<long> clock;
        readonly object sync = new object();

        public SaveStore(string root, ISaveCodec codec, FlagCatalog catalog, Func<long> clock = null)
        {
            this.root = root ?? throw new ArgumentNullException(nameof(root));
            this.codec = codec ?? throw new ArgumentNullException(nameof(codec));
            this.catalog = catalog;
            this.clock = clock ?? (() => 0L);
        }

        string PathFor(int slot) => Path.Combine(root, "sol-cobre-slot-" + slot + ".json");

        public bool Write(SaveDataV3 data)
        {
            if (data == null) return false;
            if (data.revision < 1) data.revision = 1;
            if (!SaveValidationV3.IsValid(data, out _)) return false;
            lock (sync)
            {
                string path = PathFor(data.slot);
                try
                {
                    var bytes = Encoding.UTF8.GetBytes(codec.Serialize(data));
                    if (bytes.LongLength > MaxBytes) return false;
                    Directory.CreateDirectory(root);
                    string tmp = path + ".tmp";
                    using (var stream = new FileStream(tmp, FileMode.Create, FileAccess.Write, FileShare.None))
                    { stream.Write(bytes, 0, bytes.Length); stream.Flush(true); }
                    if (File.Exists(path)) File.Replace(tmp, path, path + ".bak");
                    else File.Move(tmp, path);
                    return true;
                }
                catch (IOException) { return false; }
                catch (UnauthorizedAccessException) { return false; }
                catch (PlatformNotSupportedException) { return false; }
                // Sem fallback de apagar-e-mover: preserva o save anterior em provider limitado.
            }
        }

        public SaveLoadResult Read(int slot)
        {
            lock (sync)
            {
                if (slot < 1 || slot > 3)
                    return new SaveLoadResult { Status = SaveLoadStatus.CorruptPreserved, Reason = "slot" };
                string path = PathFor(slot);
                if (!File.Exists(path) && !File.Exists(path + ".bak"))
                    return new SaveLoadResult { Status = SaveLoadStatus.Missing };

                var primary = TryLoad(path, slot);
                // Resultados conclusivos do arquivo principal não caem para o backup.
                if (primary.Status == SaveLoadStatus.Loaded ||
                    primary.Status == SaveLoadStatus.Migrated ||
                    primary.Status == SaveLoadStatus.UnsupportedVersion)
                    return primary;

                // Principal ilegível/corrompido: tenta o backup.
                var backup = TryLoad(path + ".bak", slot);
                if (backup.Status == SaveLoadStatus.Loaded || backup.Status == SaveLoadStatus.Migrated)
                {
                    backup.Status = SaveLoadStatus.RecoveredBackup;
                    return backup;
                }
                return new SaveLoadResult { Status = SaveLoadStatus.CorruptPreserved, Reason = primary.Reason ?? "ilegível" };
            }
        }

        // Carrega um arquivo específico e classifica. Não escreve nada (preserva original).
        SaveLoadResult TryLoad(string file, int slot)
        {
            var r = new SaveLoadResult { Status = SaveLoadStatus.CorruptPreserved };
            try
            {
                if (!File.Exists(file)) { r.Reason = "ausente"; return r; }
                if (new FileInfo(file).Length > MaxBytes) { r.Reason = "acima de 1 MiB"; return r; }
                string text = File.ReadAllText(file, Encoding.UTF8);
                int version = codec.ProbeVersion(text);
                if (version > 3) { r.Status = SaveLoadStatus.UnsupportedVersion; r.Reason = "schema " + version; return r; }
                if (version == 3)
                {
                    if (codec.TryDeserializeV3(text, out var data) && SaveValidationV3.IsValid(data, out string reason))
                    { r.Status = SaveLoadStatus.Loaded; r.Data = data; return r; }
                    else { r.Reason = "v3 inválido"; return r; }
                }
                if (version == 1)
                {
                    if (codec.TryDeserializeV1(text, out var legacy))
                    {
                        var migration = SaveMigrator.FromV1(legacy, slot, clock(), catalog);
                        if (migration.Status == MigrationStatus.Migrated)
                        {
                            r.Status = SaveLoadStatus.Migrated; r.Data = migration.Data; r.Migration = migration.Report; return r;
                        }
                        if (migration.Status == MigrationStatus.Unsupported)
                        { r.Status = SaveLoadStatus.UnsupportedVersion; r.Reason = migration.Reason; return r; }
                        r.Reason = "migração: " + migration.Reason; return r;
                    }
                    r.Reason = "v1 ilegível"; return r;
                }
                r.Reason = "versão " + version; return r;
            }
            catch (IOException) { r.Reason = "io"; return r; }
            catch (UnauthorizedAccessException) { r.Reason = "acesso"; return r; }
            catch (ArgumentException) { r.Reason = "arg"; return r; }
        }
    }
}
