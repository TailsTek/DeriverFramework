using DF.Debug;
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
    public class Source
    {
        public string Name;
        public string Type;
        public int Size;
        public byte[] Data;
        public TimeSpan PackDate { get; private set; }

        public Source(string name, string type, int size, byte[] data)
        {
            Name = name;
            Type = type;
            Size = size;
            Data = data;
            PackDate = DateTime.Now.TimeOfDay;
        }
        public static Source Pack(string filepath)
        {
            byte[] data = default;
            string name = "";
            int size = 0;
            string type = "";
            try
            {
                using (var item = File.OpenRead(filepath))
                {
                    data = new byte[item.Length];
                    item.Read(data, 0, (int)item.Length);
                    var path = item.Name.Split('\\');
                    var n = path[path.Length - 1];
                    var fullname = n.Split('.').ToList();
                    type = fullname[fullname.Count - 1];
                    fullname.RemoveAt(fullname.Count - 1);
                    var newname = "";
                    for (var i = 0; i != fullname.Count; i++)
                    {
                        if (i != fullname.Count - 1)
                            newname += $"{fullname[i]}.";
                        else
                            newname += $"{fullname[i]}";
                    }
                    name = newname;
                    size = (int)item.Length; 
                }
            }
            catch (Exception ex)
            {
                LogFramework.WriteNewLog($"Failed pack file.", PackageManager.CClass, PackageManager.Instance, null, LogEnums.ResultCode.ErrorMethod, ConsoleColor.Red, ex);
                return null;
            }
            var s = new Source(name, type, size, data);
            LogFramework.WriteNewLog($"{name}.{type} is packed.", PackageManager.CClass, PackageManager.Instance, null, LogEnums.ResultCode.OKMethod);
            return s;
        }
        public void Unpack(string folderpath)
        {
            var name = $"{Name}.{Type}";
            var data = Data;
            try
            {
                if (!Directory.Exists(folderpath))
                {
                    Directory.CreateDirectory(folderpath);
                }
                using (var item = File.OpenWrite($"{folderpath}\\{name}"))
                {
                    item.Write(data, 0, data.Length);
                }
                LogFramework.WriteNewLog($"{name} is unpacked.", PackageManager.CClass, PackageManager.Instance, null, LogEnums.ResultCode.OKMethod);
            }
            catch (Exception ex)
            {
                LogFramework.WriteNewLog($"Failed unpack file.", PackageManager.CClass, PackageManager.Instance, null, LogEnums.ResultCode.ErrorMethod, ConsoleColor.Red, ex);
            }
            
        }
    }
}
