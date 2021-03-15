using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DF.Classes
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
            var res = string.Format("{0}, {1}", Name, loaded);
            return res;
        }

        public static bool operator !=(DynamicLibrary v1, DynamicLibrary v2)
        {
            return v1.Name == v2.Name && v1.Raw == v2.Raw;
        }
        public static bool operator ==(DynamicLibrary v1, DynamicLibrary v2)
        {
            return v1.Name == v2.Name && v1.Raw == v2.Raw;
        }
        public bool Load()
        {
            bool ok;
            if (Raw != null)
            {
                try
                {
                    AppDomain.CurrentDomain.Load(Raw);
                }
                catch
                {
                    //ok = false;
                    Loaded = false;
                }
                Loaded = true;
                ok = true;
            }
            else
            {
                ok = false;
                Loaded = false;
            }
            return ok;
        }
        public DynamicLibrary(string Name, string Ver, byte[] Raw)
        {
            this.Name = Name;
            this.Ver = Ver;
            this.Raw = Raw;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is null)
            {
                return false;
            }

            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        /*
        public DynamicLibrary(string Name, string Ver, string respath)
        {
            this.Name = Name;
            this.Ver = Ver;
            var st = Application.GetResourceStream(new Uri(respath, UriKind.Relative));
            Raw = new byte[st.Stream.Length];
            st.Stream.Read(Raw, 0, (int)st.Stream.Length);
        }
        */
    }
}
