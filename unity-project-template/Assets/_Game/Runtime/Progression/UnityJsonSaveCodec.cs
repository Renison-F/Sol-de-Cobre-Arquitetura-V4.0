using System;
using UnityEngine;
namespace SolDeCobre.Progression
{
    // Codec de produção: JsonUtility lê/escreve as chaves snake_case do contrato.
    // JSON malformado/truncado lança e é tratado como ilegível (recuperação/preservação).
    public sealed class UnityJsonSaveCodec : ISaveCodec
    {
        [Serializable] sealed class VersionProbe { public int schema_version; public int schemaVersion; }

        public string Serialize(SaveDataV3 data) => JsonUtility.ToJson(data, true);

        public int ProbeVersion(string text)
        {
            if (string.IsNullOrEmpty(text)) return -1;
            try
            {
                var p = JsonUtility.FromJson<VersionProbe>(text);
                if (p == null) return -1;
                if (p.schema_version > 0) return p.schema_version; // v3+
                return p.schemaVersion > 0 ? p.schemaVersion : -1;  // v1 legado
            }
            catch (ArgumentException) { return -1; }
        }

        public bool TryDeserializeV3(string text, out SaveDataV3 data)
        {
            data = null;
            try
            {
                var d = JsonUtility.FromJson<SaveDataV3>(text);
                if (d == null || d.options == null) return false;
                if (d.flags == null) d.flags = Array.Empty<string>();
                if (d.atlas == null) d.atlas = Array.Empty<string>();
                data = d;
                return true;
            }
            catch (ArgumentException) { return false; }
        }

        public bool TryDeserializeV1(string text, out SaveSnapshot data)
        {
            data = null;
            try
            {
                var d = JsonUtility.FromJson<SaveSnapshot>(text);
                if (d == null) return false;
                if (d.questFlags == null) d.questFlags = Array.Empty<string>();
                data = d;
                return true;
            }
            catch (ArgumentException) { return false; }
        }
    }
}
