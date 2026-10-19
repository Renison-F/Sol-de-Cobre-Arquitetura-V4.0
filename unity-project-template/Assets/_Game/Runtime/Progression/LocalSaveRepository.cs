using System;
using System.IO;
using System.Text;
using UnityEngine;
namespace SolDeCobre.Progression
{
    public enum SaveReadStatus { Missing, Loaded, RecoveredBackup, InvalidPreserved, UnsupportedVersion }
    // Compatibility sample for the V2 schema. Target V3 schema is in 11-contratos/.
    // Read never silently replaces corrupt data with a new campaign.
    public sealed class LocalSaveRepository
    {
        const int MaxBytes=1048576;
        readonly string path;
        readonly object sync=new object();
        public LocalSaveRepository(string root=null)
        { path=Path.Combine(root ?? Application.persistentDataPath,"sol-cobre-save.json"); }
        public SaveReadStatus Read(out SaveSnapshot result)
        {
            lock(sync)
            {
                result=null;
                if(!File.Exists(path) && !File.Exists(path+".bak")) return SaveReadStatus.Missing;
                var status=TryRead(path,out result);
                if(status==SaveReadStatus.Loaded || status==SaveReadStatus.UnsupportedVersion) return status;
                if(TryRead(path+".bak",out result)==SaveReadStatus.Loaded) return SaveReadStatus.RecoveredBackup;
                return SaveReadStatus.InvalidPreserved;
            }
        }
        static SaveReadStatus TryRead(string file,out SaveSnapshot result)
        {
            result=null;
            try
            {
                if(!File.Exists(file) || new FileInfo(file).Length>MaxBytes) return SaveReadStatus.InvalidPreserved;
                var parsed=JsonUtility.FromJson<SaveSnapshot>(File.ReadAllText(file,Encoding.UTF8));
                if(parsed==null) return SaveReadStatus.InvalidPreserved;
                if(parsed.schemaVersion!=1) return SaveReadStatus.UnsupportedVersion;
                if(!Valid(parsed)) return SaveReadStatus.InvalidPreserved;
                result=parsed; return SaveReadStatus.Loaded;
            }
            catch(IOException) { return SaveReadStatus.InvalidPreserved; }
            catch(UnauthorizedAccessException) { return SaveReadStatus.InvalidPreserved; }
            catch(ArgumentException) { return SaveReadStatus.InvalidPreserved; }
        }
        static bool Valid(SaveSnapshot s)
        {
            if(s==null || s.schemaVersion!=1 || s.chapterId==null || s.chapterId.Length>64 ||
                s.checkpointId==null || s.checkpointId.Length>64 ||
                (s.heroId!="mara" && s.heroId!="ze") || s.questFlags==null || s.questFlags.Length>4096) return false;
            foreach(var flag in s.questFlags) if(string.IsNullOrEmpty(flag) || flag.Length>128) return false;
            return true;
        }
        public bool Write(SaveSnapshot snapshot)
        {
            lock(sync)
            {
                if(!Valid(snapshot)) return false;
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                    var data=Encoding.UTF8.GetBytes(JsonUtility.ToJson(snapshot,true));
                    if(data.Length>MaxBytes) return false;
                    using(var stream=new FileStream(path+".tmp",FileMode.Create,FileAccess.Write,FileShare.None))
                    { stream.Write(data,0,data.Length); stream.Flush(true); }
                    if(File.Exists(path)) File.Replace(path+".tmp",path,path+".bak");
                    else File.Move(path+".tmp",path);
                    return true;
                }
                catch(IOException) { return false; }
                catch(UnauthorizedAccessException) { return false; }
                catch(PlatformNotSupportedException) { return false; }
                // No delete-and-move fallback: preserve the previous save on unsupported providers.
            }
        }
    }
}
