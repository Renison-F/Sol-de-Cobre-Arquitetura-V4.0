using System;
using UnityEngine;
namespace SolDeCobre.Progression
{
    // Composição de produção do save: três slots sob Application.persistentDataPath,
    // codec JsonUtility, catálogo de flags da campanha e relógio Unix para written_at.
    // O save legado (LocalSaveRepository) continua como exemplo histórico do schema 1.
    public sealed class SaveSystem
    {
        readonly SaveStore store;

        public SaveSystem(string root = null, FlagCatalog catalog = null)
        {
            store = new SaveStore(
                root ?? Application.persistentDataPath,
                new UnityJsonSaveCodec(),
                catalog ?? FlagCatalog.Default(),
                NowUnix);
        }

        static long NowUnix() => DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        public SaveLoadResult Read(int slot) => store.Read(slot);
        public bool Write(SaveDataV3 data) => store.Write(data);
    }
}
