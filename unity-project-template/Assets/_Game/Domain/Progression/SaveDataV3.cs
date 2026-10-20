using System;
namespace SolDeCobre.Progression
{
    // Alvo V3 (11-contratos/save.schema.json). Campos em snake_case para que o
    // JsonUtility do Unity leia/escreva as mesmas chaves do contrato.
    [Serializable] public sealed class SaveOptions
    {
        public bool assist = false;
        public bool subtitles = true;
        public bool reduced_flashes = true;
        public SaveOptions Clone() =>
            new SaveOptions { assist = assist, subtitles = subtitles, reduced_flashes = reduced_flashes };
    }

    [Serializable] public sealed class SaveDataV3
    {
        public int schema_version = 3;
        public int slot = 1;
        public int revision = 1;
        public string chapter = "LVL-01";
        public string checkpoint = "";
        public string hero = "mara";
        public string[] flags = Array.Empty<string>();
        public string[] atlas = Array.Empty<string>();
        public long written_at = 0;
        public SaveOptions options = new SaveOptions();
    }
}
