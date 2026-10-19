namespace SolDeCobre.Narrative
{
    public static class QuestRules
    {
        public static bool IsReady(QuestState state, QuestDefinition definition) =>
            state != null && definition != null &&
            state.Ready(definition.requiredFlags ?? System.Array.Empty<string>());
    }
}
