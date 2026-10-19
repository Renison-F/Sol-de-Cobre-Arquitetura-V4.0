namespace SolDeCobre.Progression
{
    // Serialização injetável: o Unity implementa com JsonUtility; os testes usam um
    // codec engine-free. Mantém o SaveStore (I/O + roteamento) livre de engine e testável.
    public interface ISaveCodec
    {
        string Serialize(SaveDataV3 data);
        // Versão do schema lida do texto, ou -1 se ilegível. v1 legado reporta 1.
        int ProbeVersion(string text);
        bool TryDeserializeV3(string text, out SaveDataV3 data);
        bool TryDeserializeV1(string text, out SaveSnapshot data);
    }
}
