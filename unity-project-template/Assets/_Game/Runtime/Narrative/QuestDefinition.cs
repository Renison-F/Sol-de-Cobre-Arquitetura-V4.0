using UnityEngine;
namespace SolDeCobre.Narrative
{
    [CreateAssetMenu(menuName="Sol de Cobre/Quest Definition")]
    public sealed class QuestDefinition : ScriptableObject
    {
        public string questId;
        [TextArea(2, 5)] public string objective;
        public string chapterId;
        public string[] requiredFlags;
    }
}
