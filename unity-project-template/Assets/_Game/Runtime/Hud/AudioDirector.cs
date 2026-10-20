using System;
using UnityEngine;
namespace SolDeCobre.Hud
{
    // Toca cues em canais separados (música/efeitos/voz), para que o aviso de ameaça
    // continue audível com a música desligada. Prioridade/abafamento do pool é a regra
    // pura de AudioCueBus; aqui só a reprodução e o volume por canal. Mixagem fina
    // (AudioMixer/ducking) é configurada na cena.
    public sealed class AudioDirector : MonoBehaviour
    {
        [Serializable] struct ClipEntry { public string id; public AudioClip clip; }
        [SerializeField] AudioSource music, sfx, voice;
        [SerializeField] ClipEntry[] clips;

        AudioSource SourceFor(AudioBus bus) =>
            bus == AudioBus.Music ? music : bus == AudioBus.Voice ? voice : sfx;

        public void Play(string cueId, AudioBus bus)
        {
            var clip = Find(cueId);
            var source = SourceFor(bus);
            if (clip != null && source != null) source.PlayOneShot(clip);
        }

        public void SetBusVolume(AudioBus bus, float volume01)
        {
            var source = SourceFor(bus);
            if (source != null) source.volume = Mathf.Clamp01(volume01);
        }

        AudioClip Find(string id)
        {
            if (clips == null) return null;
            foreach (var e in clips) if (e.id == id) return e.clip;
            return null;
        }
    }
}
