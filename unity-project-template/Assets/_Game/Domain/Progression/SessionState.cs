using System.Collections.Generic;
using System.Linq;
namespace SolDeCobre.Progression
{
    // Estado de sessão em jogo: a fonte do snapshot coerente para SaveCheckpoint e o
    // destino da restauração em StartSession. Converte de/para o contrato V3. Revisão
    // cresce por checkpoint (não por frame); flags/atlas sem duplicar.
    public sealed class SessionState
    {
        public int Slot = 1;
        public int Revision = 1;
        public string Chapter = "LVL-01";
        public string Checkpoint = "";
        public string HeroId = "mara";
        public readonly List<string> Flags = new List<string>();
        public readonly List<string> Atlas = new List<string>();
        public SaveOptions Options = new SaveOptions();

        public bool GrantFlag(string flag)
        {
            if (string.IsNullOrEmpty(flag) || Flags.Contains(flag)) return false;
            Flags.Add(flag); return true;
        }
        public bool GrantAtlas(string id)
        {
            if (string.IsNullOrEmpty(id) || Atlas.Contains(id)) return false;
            Atlas.Add(id); return true;
        }
        public bool SwitchHero(string heroId)
        {
            if (heroId != "mara" && heroId != "ze") return false;
            HeroId = heroId; return true;
        }
        // Revisão crescente ao gravar um checkpoint (serialização de saves concorrentes).
        public int NextRevision() => ++Revision;

        public SaveDataV3 ToSave(long writtenAt) => new SaveDataV3
        {
            schema_version = 3,
            slot = Slot,
            revision = Revision < 1 ? 1 : Revision,
            chapter = Chapter,
            checkpoint = Checkpoint ?? "",
            hero = HeroId,
            flags = Flags.Distinct().ToArray(),
            atlas = Atlas.Distinct().ToArray(),
            written_at = writtenAt < 0 ? 0 : writtenAt,
            options = Options?.Clone() ?? new SaveOptions()
        };

        public static SessionState FromSave(SaveDataV3 data)
        {
            var state = new SessionState
            {
                Slot = data.slot,
                Revision = data.revision,
                Chapter = data.chapter,
                Checkpoint = data.checkpoint ?? "",
                HeroId = data.hero,
                Options = data.options?.Clone() ?? new SaveOptions()
            };
            if (data.flags != null) state.Flags.AddRange(data.flags);
            if (data.atlas != null) state.Atlas.AddRange(data.atlas);
            return state;
        }
    }
}
