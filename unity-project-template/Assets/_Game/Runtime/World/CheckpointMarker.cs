using UnityEngine;
using SolDeCobre.Core;
namespace SolDeCobre.World
{
    public sealed class CheckpointMarker : MonoBehaviour
    {
        [SerializeField] string checkpointId;
        public string CheckpointId => checkpointId;
        public void Activate(GameSignals signals)
        {
            if (signals == null || string.IsNullOrWhiteSpace(checkpointId)) return;
            signals.OnCheckpointReached(checkpointId);
        }
    }
}
