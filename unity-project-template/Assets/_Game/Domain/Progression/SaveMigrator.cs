using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace SolDeCobre.Progression
{
    public enum MigrationStatus { Migrated, Unsupported, Corrupt }

    // Relatório externo da migração: avisos e itens antigos desconhecidos guardados
    // para investigação — NUNCA convertidos em flags/compras/habilidades.
    public sealed class MigrationReport
    {
        public readonly List<string> Warnings = new List<string>();
        public readonly List<string> UnknownFlags = new List<string>();
    }

    public sealed class MigrationResult
    {
        public MigrationStatus Status;
        public SaveDataV3 Data;
        public readonly MigrationReport Report = new MigrationReport();
        public string Reason;
    }

    // Migrador puro schema1 -> schema3. Mapeia campo a campo, preserva a entrada
    // original (o chamador não deve apagar o arquivo antigo) e produz resultado
    // Migrated/Unsupported/Corrupt (03-save-e-migracao.md, 11-conteudo-e-save.md).
    public static class SaveMigrator
    {
        static readonly Regex Chapter = new Regex(@"\ALVL-(0[1-9]|10)\z", RegexOptions.CultureInvariant);

        public static MigrationResult FromV1(SaveSnapshot legacy, int slot, long writtenAt, FlagCatalog catalog)
        {
            var result = new MigrationResult();
            if (legacy == null) { result.Status = MigrationStatus.Corrupt; result.Reason = "null"; return result; }
            if (legacy.schemaVersion != 1)
            {
                // Versão futura/desconhecida não é migrada por este caminho.
                result.Status = legacy.schemaVersion > 1 ? MigrationStatus.Unsupported : MigrationStatus.Corrupt;
                result.Reason = "schemaVersion=" + legacy.schemaVersion;
                return result;
            }
            if (slot < 1 || slot > 3) { result.Status = MigrationStatus.Corrupt; result.Reason = "slot"; return result; }
            if (legacy.chapterId == null || !Chapter.IsMatch(legacy.chapterId))
            { result.Status = MigrationStatus.Corrupt; result.Reason = "chapter inválido"; return result; }
            if (legacy.heroId != "mara" && legacy.heroId != "ze")
            { result.Status = MigrationStatus.Corrupt; result.Reason = "hero inválido"; return result; }

            var data = new SaveDataV3
            {
                schema_version = 3,
                slot = slot,
                revision = 1,
                chapter = legacy.chapterId,
                hero = legacy.heroId,
                atlas = System.Array.Empty<string>(),
                written_at = writtenAt < 0 ? 0 : writtenAt,
                options = new SaveOptions() // defaults documentados: assist=false, subtitles=true, reduced_flashes=true
            };

            // Checkpoint inválido volta à entrada segura do mesmo capítulo, com aviso.
            string cp = legacy.checkpointId ?? "";
            if (cp.Length > 64)
            {
                result.Report.Warnings.Add("checkpoint inválido; entrada segura do capítulo " + data.chapter);
                cp = "";
            }
            data.checkpoint = cp;

            // Flags: conhecidas são preservadas (sem duplicar); desconhecidas vão só ao relatório.
            var known = new List<string>();
            var seen = new HashSet<string>();
            if (legacy.questFlags != null)
            {
                foreach (var f in legacy.questFlags)
                {
                    if (string.IsNullOrEmpty(f)) continue;
                    if (catalog != null && catalog.Contains(f))
                    {
                        if (seen.Add(f)) known.Add(f);
                    }
                    else result.Report.UnknownFlags.Add(f);
                }
            }
            data.flags = known.ToArray();

            if (!SaveValidationV3.IsValid(data, out string reason))
            { result.Status = MigrationStatus.Corrupt; result.Reason = "pós-migração: " + reason; return result; }

            result.Status = MigrationStatus.Migrated;
            result.Data = data;
            return result;
        }
    }
}
