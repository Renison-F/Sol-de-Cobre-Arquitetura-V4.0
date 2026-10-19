using System.IO;
using System.Text;
using UnityEngine;
namespace SolDeCobre.Hud
{
    // Preferências locais por perfil (escala/opacidade/legendas/volumes/canhoto) num
    // arquivo separado do save (telas-e-fluxos.md). As três opções do save em si
    // (assist/subtitles/reduced_flashes) continuam no SaveDataV3. Falha de I/O preserva
    // o arquivo anterior e devolve defaults — nunca apaga silenciosamente.
    public sealed class PreferencesStore
    {
        readonly string path;

        public PreferencesStore(string root = null)
        {
            path = Path.Combine(root ?? Application.persistentDataPath, "sol-cobre-prefs.json");
        }

        public PlayerPreferences Load()
        {
            try
            {
                if (File.Exists(path))
                {
                    var p = JsonUtility.FromJson<PlayerPreferences>(File.ReadAllText(path, Encoding.UTF8));
                    if (p != null) { p.Clamp(); return p; }
                }
            }
            catch (IOException) { }
            catch (System.UnauthorizedAccessException) { }
            catch (System.ArgumentException) { }
            return new PlayerPreferences();
        }

        public bool Save(PlayerPreferences prefs)
        {
            if (prefs == null) return false;
            prefs.Clamp();
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                string tmp = path + ".tmp";
                File.WriteAllText(tmp, JsonUtility.ToJson(prefs, true), Encoding.UTF8);
                if (File.Exists(path)) File.Replace(tmp, path, null);
                else File.Move(tmp, path);
                return true;
            }
            catch (IOException) { return false; }
            catch (System.UnauthorizedAccessException) { return false; }
            catch (System.PlatformNotSupportedException) { return false; }
        }
    }
}
