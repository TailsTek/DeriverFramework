using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DeriverFramework.Classes
{
    public class DynamicLibrary
    {
        public string Name;
        public string Ver;
        public bool Loaded;
        public byte[] Raw;

        public override string ToString()
        {
            string loaded = "";
            if (Loaded)
                loaded = "Loaded";
            else
            {
                loaded = "Unloaded";
            }
            return string.Format("{0}, {1}", Name, loaded);
        }
        public static bool operator !=(DynamicLibrary v1, DynamicLibrary v2)
        {
            return v1.Name == v2.Name && v1.Raw == v2.Raw;
        }
        public static bool operator ==(DynamicLibrary v1, DynamicLibrary v2)
        {
            return v1.Name == v2.Name && v1.Raw == v2.Raw;
        }

        public DynamicLibrary(string Name, string Ver, byte[] Raw)
        {
            this.Name = Name;
            this.Ver = Ver;
            this.Raw = Raw;
        }
        public DynamicLibrary(byte[] Raw)
        {
            this.Name = "DeriverFramework";
            this.Ver = "0";
            this.Raw = Raw;
        }
        public DynamicLibrary(string Name, string Ver, string respath)
        {
            this.Name = Name;
            this.Ver = Ver;
            var st = Application.GetResourceStream(new Uri(respath, UriKind.Relative));
            Raw = new byte[st.Stream.Length];
            st.Stream.Read(Raw, 0, (int)st.Stream.Length);
        }
        public DynamicLibrary(string respath)
        {
            this.Name = "DeriverFramework";
            this.Ver = "0";
            var st = Application.GetResourceStream(new Uri(respath, UriKind.Relative));
            if (st != null)
            {
                Raw = new byte[st.Stream.Length];
                st.Stream.Read(Raw, 0, (int)st.Stream.Length);
            }
        }
    }
}
