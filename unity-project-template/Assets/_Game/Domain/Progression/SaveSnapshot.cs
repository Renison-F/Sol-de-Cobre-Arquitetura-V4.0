using System;
namespace SolDeCobre.Progression
{
    [Serializable] public sealed class SaveSnapshot
    {
        public int schemaVersion = 1;
        public string chapterId = "LVL-01";
        public string checkpointId = "";
        public string heroId = "mara";
        public string[] questFlags = Array.Empty<string>();
    }
}
