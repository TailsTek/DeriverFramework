using DF.Classes;
using DF.Debug;
using DF.Enums;
using DF.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Tools
{
    public sealed class PackageManager : ICClassObject
    {
        internal static PackageManager Instance;

        internal static CClass CClass;
        public PackageManager()
        {
            Instance = this;
            CClass.CheckCreateAdd(this, nameof(PackageManager));
        }
        public List<ProtectedPackage> Packages { get; private set; } = new List<ProtectedPackage>();
        public void AddPkg(ProtectedPackage pp)
        {
            Packages.Add(pp);
            LogFramework.WriteNewLog($"{pp.Name} loaded to package list.", CClass, this, null, LogEnums.ResultCode.OKMethod);
        }
        public void AddToPkg(string pkg, Source s)
        {
            bool finded = false;
            foreach (var item in Packages)
            {
                if (item.Name == pkg)
                {
                    finded = true;
                    item.Sources.Add(s);
                    LogFramework.WriteNewLog($"{s.Name} added to {item.Name} in package list.", CClass, this, null, LogEnums.ResultCode.OKMethod);
                }
            }
            if (!finded)
            {
                LogFramework.WriteNewLog($"Unknown {pkg} in package list.", CClass, this, null, LogEnums.ResultCode.ErrorMethod);
            }
        }
        public Source GetSource(string pkg, string file)
        {
            bool finded = false;
            Source s = default;
            foreach (var item in Packages)
            {
                if (item.Name == pkg)
                {
                    finded = true;
                    foreach (var itemm in item.Sources)
                    {
                        if (itemm.Name == file)
                        {
                            s = itemm;
                        }
                    }
                }
            }
            if (!finded)
            {
                LogFramework.WriteNewLog($"Unknown {pkg} in package list.", CClass, this, null, LogEnums.ResultCode.ErrorMethod);
                return null;
            }
            return s;
        }
        public ProtectedPackage GetPkg(string pkg)
        {
            bool finded = false;
            ProtectedPackage pp = default;
            foreach (var item in Packages)
            {
                if (item.Name == pkg)
                {
                    finded = true;
                    pp = item;
                }
            }
            if (!finded)
            {
                LogFramework.WriteNewLog($"Unknown {pkg} in package list.", CClass, this, null, LogEnums.ResultCode.ErrorMethod);
                return null;
            }
            return pp;
        }
        public void Import(string pkgpath)
        {
            Packages.Add(ProtectedPackage.Import(pkgpath));
        }
        public void Export(string pkgname, string folderpath)
        {
            foreach(var item in Packages)
            {
                if (item.Name == pkgname)
                {
                    ProtectedPackage.Export(item, folderpath);
                }
            }
        }

        string ICClassObject.ObjectName()
        {
            return nameof(PackageManager);
        }

        CClassObjectType ICClassObject.Type()
        {
            return CClassObjectType.PackageManager;
        }

        string ICClassObject.Invoke(params string[] parameters)
        {
            return "";
        }

        void ICClassObject.SetCClass(CClass cclass)
        {
            CClass = cclass;
        }
        object ICClassObject.GetObject()
        {
            return this;
        }

        int ICClassObject.GetHashCode()
        {
            return GetHashCode();
        }
    }
}
