using System.Collections.Generic;
namespace SolDeCobre.Narrative
{
    // Serviço de regras; independente de GameObject e de interface gráfica.
    public sealed class QuestState
    {
        readonly HashSet<string> flags = new HashSet<string>();
        public bool HasFlag(string id) => flags.Contains(id);
        public bool GrantFlag(string id) => !string.IsNullOrWhiteSpace(id) && flags.Add(id);
        public bool Ready(IEnumerable<string> requiredFlags)
        {
            if(requiredFlags == null) return false;
            foreach(var flag in requiredFlags)
                if(!flags.Contains(flag)) return false;
            return true;
        }
        public string[] Snapshot() { var array=new string[flags.Count];flags.CopyTo(array);return array; }
    }
}
