using DF.Debug;
using DF.In;
using DF.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Classes
{
    [Serializable]
    public class ProtectedPackage
    {
        public const string FileType = "prkg";
        public string Name;
        public List<Source> Sources;

        public ProtectedPackage()
        {
            Sources = new List<Source>();
        }
        public ProtectedPackage(List<Source> s)
        {
            Sources = new List<Source>(s);
        }
        public void Pack(string filepath)
        {
            Sources.Add(Source.Pack(filepath));
        }
        public void Unpack(string filename, string folderpath)
        {
            foreach (var item in Sources)
            {
                if ($"{item.Name}.{item.Type}" == filename)
                {
                    item.Unpack(folderpath);
                }
            }
        }
        public static ProtectedPackage Import(string pkgpath)
        {
            ProtectedPackage pp = default;
            try
            {
                byte[] data;
                using (var item = File.OpenRead($"{pkgpath}"))
                {
                    data = new byte[item.Length];
                    item.Read(data, 0, (int)item.Length);

                    var cm = new CacheMemory(data, "test");
                    cm.Decompress();
                    cm.Decrypt();

                    cm.Read(data, 0, (int)cm.Length);
                    data = cm.ToByteArray();
                }
                pp = data.ByteArrayToObject<ProtectedPackage>();
            }
            catch (Exception ex)
            {
                LogFramework.WriteNewLog("Failed import package.", PackageManager.CClass, PackageManager.Instance, null, LogEnums.ResultCode.ErrorMethod, ConsoleColor.Red, ex);
            }
            LogFramework.WriteNewLog("Package imported.", PackageManager.CClass, PackageManager.Instance, null, LogEnums.ResultCode.OKMethod);
            return pp;
        }
        public static void Export(ProtectedPackage pkg, string folderpath)
        {
            try
            {
                if (!Directory.Exists(folderpath))
                {
                    Directory.CreateDirectory(folderpath);
                }
                var path = $@"{folderpath}\{pkg.Name}.{FileType}";
                using (var item = File.OpenWrite(path))
                {
                    var data = pkg.ObjectToByteArray();
                    CacheMemory cm = new CacheMemory(data, "test");
                    cm.Encrypt();
                    cm.Compress();
                    data = cm.ToByteArray();
                    item.Write(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            {
                LogFramework.WriteNewLog("Failed import package.", PackageManager.CClass, PackageManager.Instance, null, LogEnums.ResultCode.ErrorMethod, ConsoleColor.Red, ex);
                return;
            }
            LogFramework.WriteNewLog("Package exported.", PackageManager.CClass, PackageManager.Instance, null, LogEnums.ResultCode.OKMethod);
        }
    }
}
