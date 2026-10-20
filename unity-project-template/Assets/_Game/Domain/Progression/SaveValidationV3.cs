using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace SolDeCobre.Progression
{
    // Valida as invariantes do contrato V3 antes de gravar ou após carregar.
    // Não corrige: apenas aprova/reprova com motivo (diagnóstico recuperável).
    public static class SaveValidationV3
    {
        static readonly Regex Chapter = new Regex(@"\ALVL-(0[1-9]|10)\z", RegexOptions.CultureInvariant);
        static readonly Regex Atlas = new Regex(@"\AATL-(0[1-9]|10)-0[1-3]\z", RegexOptions.CultureInvariant);

        public static bool IsValid(SaveDataV3 s, out string reason)
        {
            reason = null;
            if (s == null) { reason = "null"; return false; }
            if (s.schema_version != 3) { reason = "schema_version"; return false; }
            if (s.slot < 1 || s.slot > 3) { reason = "slot"; return false; }
            if (s.revision < 1) { reason = "revision"; return false; }
            if (s.chapter == null || !Chapter.IsMatch(s.chapter)) { reason = "chapter"; return false; }
            if (s.checkpoint == null || s.checkpoint.Length > 64) { reason = "checkpoint"; return false; }
            if (s.hero != "mara" && s.hero != "ze") { reason = "hero"; return false; }
            if (s.written_at < 0) { reason = "written_at"; return false; }
            if (s.options == null) { reason = "options"; return false; }

            if (s.flags == null || s.flags.Length > 4096) { reason = "flags"; return false; }
            var seenFlags = new HashSet<string>();
            foreach (var f in s.flags)
            {
                if (string.IsNullOrEmpty(f) || f.Length > 128) { reason = "flag item"; return false; }
                if (!seenFlags.Add(f)) { reason = "flag duplicada"; return false; }
            }

            if (s.atlas == null || s.atlas.Length > 30) { reason = "atlas"; return false; }
            var seenAtlas = new HashSet<string>();
            foreach (var a in s.atlas)
            {
                if (a == null || !Atlas.IsMatch(a)) { reason = "atlas item"; return false; }
                if (!seenAtlas.Add(a)) { reason = "atlas duplicado"; return false; }
            }
            return true;
        }
    }
}
