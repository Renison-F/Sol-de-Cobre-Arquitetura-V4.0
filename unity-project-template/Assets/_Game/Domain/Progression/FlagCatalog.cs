using System.Collections.Generic;
namespace SolDeCobre.Progression
{
    // Catálogo de flags permitidas. Semeado a partir dos "beats" de
    // 11-contratos/campanha.json (um flag por beat); regenerar pelo importador
    // de conteúdo quando a campanha mudar. O migrador usa este catálogo para
    // separar flags conhecidas (preservadas) de desconhecidas (só no relatório).
    public sealed class FlagCatalog
    {
        readonly HashSet<string> allowed;
        public FlagCatalog(IEnumerable<string> flags) { allowed = new HashSet<string>(flags); }
        public bool Contains(string flag) => flag != null && allowed.Contains(flag);
        public int Count => allowed.Count;

        // Padrão derivado da campanha V4 (60 flags).
        public static FlagCatalog Default() => new FlagCatalog(DefaultFlags);
        public static readonly string[] DefaultFlags =
        {
            "POCO_EXAMINADO",
            "LACO_APRENDIDO",
            "DESVIO_ENCONTRADO",
            "PATIO_ABERTO",
            "BOSS_01_DEFEATED",
            "LVL_01_COMPLETE",
            "ECO_1",
            "ECO_2",
            "DUTO_VISTO",
            "ECO_3",
            "BOSS_02_DEFEATED",
            "LVL_02_COMPLETE",
            "ZE_JOINED",
            "ELEVADOR_ESTAVEL",
            "LUNA_RESCUED",
            "CONDENSADOR_RECUPERADO",
            "BOSS_03_DEFEATED",
            "LVL_03_COMPLETE",
            "SINO_ATIVO",
            "TROVICO_JOINED",
            "EVACUACAO_VISTA",
            "TRANSMISSOR_LOCALIZADO",
            "BOSS_04_DEFEATED",
            "LVL_04_COMPLETE",
            "TREM_EMBARCADO",
            "VAGOES_CRUZADOS",
            "FITA_RECUPERADA",
            "CHAVE_OBSERVATORIO",
            "BOSS_05_DEFEATED",
            "LVL_05_COMPLETE",
            "OASIS_ACESSO",
            "CANAL_1",
            "CANAL_2",
            "CANAL_3",
            "BOSS_06_DEFEATED",
            "LVL_06_COMPLETE",
            "ZE_RECONHECIDO",
            "ATERRAMENTO_1",
            "ZE_CONFESSED",
            "ANTENA_ISOLADA",
            "BOSS_07_DEFEATED",
            "LVL_07_COMPLETE",
            "OBSERVATORIO_ABERTO",
            "LENTES_ALINHADAS",
            "ANEL_CRUZADO",
            "ISADORA_CONTACTED",
            "BOSS_08_DEFEATED",
            "LVL_08_COMPLETE",
            "CIDADE_ENTRADA",
            "CANAL_SEGURANCA_1",
            "CANAL_SEGURANCA_2",
            "CANAL_SEGURANCA_3",
            "BOSS_09_DEFEATED",
            "LVL_09_COMPLETE",
            "NUCLEO_ACESSO",
            "BARAO_CONFRONTED",
            "NUCLEO_BLINDAGEM_OFF",
            "NUCLEO_ESTAVEL",
            "ISADORA_RESCUED",
            "LVL_10_COMPLETE",
        };
    }
}
