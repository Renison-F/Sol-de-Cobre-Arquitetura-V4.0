using System.Collections.Generic;
namespace SolDeCobre.Hud
{
    // Prioridade do pool de áudio (06-audio-...md):
    // aviso de dano > diálogo > interação > ambiente > detalhe.
    public enum CuePriority { Detail = 0, Ambience = 1, Interaction = 2, Dialogue = 3, DamageWarning = 4 }
    public enum AudioBus { Music, Sfx, Voice }

    public readonly struct AudioCue
    {
        public readonly string Id;
        public readonly CuePriority Priority;
        public readonly AudioBus Bus;
        public AudioCue(string id, CuePriority priority, AudioBus bus) { Id = id; Priority = priority; Bus = bus; }
    }

    // Arbitra qual cue toca quando os canais são limitados: a maior prioridade vence
    // (abafa as demais). Mix separado: um cue fora da música continua audível com a
    // música desligada — ameaça visual comunicada por som mesmo sem trilha.
    public sealed class AudioCueBus
    {
        readonly List<AudioCue> active = new List<AudioCue>();

        public void Raise(AudioCue cue)
        {
            Clear(cue.Id);
            active.Add(cue);
        }
        public void Clear(string id) => active.RemoveAll(c => c.Id == id);
        public void ClearAll() => active.Clear();

        public bool TryPickHighest(out AudioCue top)
        {
            top = default;
            bool found = false;
            foreach (var c in active) // empate mantém o primeiro levantado (ordem estável)
                if (!found || c.Priority > top.Priority) { top = c; found = true; }
            return found;
        }

        public static bool AudibleWhenMusicMuted(AudioCue cue) => cue.Bus != AudioBus.Music;
    }
}
