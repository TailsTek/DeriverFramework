using DF.Enums;
using DF.Interfaces;
using DF.Structs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DF.Classes
{
    public class CClass
    {
        /// <summary>
        /// Container of cclasses.
        /// </summary>
        internal static ExtraArray<CClass> CClasses { get; private set; }
        /// <summary>
        /// Inserting CClass.
        /// </summary>
        /// <param name="cclass"></param>
        internal static void InsertCClass(CClass cclass)
        {
            bool finded = false;
            for (var i = 0; i != CClasses.Count; i++)
            {
                if (CClasses[i].Name == cclass.Name)
                {
                    CClasses[i].ReplaceContainer(cclass.Objects);
                    finded = true;
                }
            }
            if (!finded)
            {
                CClasses.Add(cclass);
            }
        }
        /// <summary>
        /// Checking cclass, if not found creating new cclass & adding object in cclass.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="cclassname"></param>
        internal static void CheckCreateAdd(ICClassObject obj, string cclassname)
        {
            if (ExtraArray<CClass>.IsNull(CClasses))
            {
                CClasses = new ExtraArray<CClass>(Array.Empty<CClass>());
            }

            CClass cclass = default;
            bool finded = false;
            foreach (var item in CClasses.Array.Span)
            {
                if (item.Name == cclassname)
                {
                    cclass = item;
                    finded = true;
                }
            }
            if (!finded)
                cclass = new CClass(cclassname);
            foreach (var item in cclass.Objects)
            {
                var name1 = item.ObjectName();
                var name2 = obj.ObjectName();
                if (name1 == name2)
                {
                    throw new Exception("This object currently in container of cclass.");
                }
            }
            obj.SetCClass(cclass);
            cclass.AddObject(obj);
            InsertCClass(cclass);
        }
        internal static ICClassObject GetCClassObject(string cclassname, CClassObjectType type, string objname)
        {
            ICClassObject Obj = default;

            var cclass = GetCClass(cclassname);
            for (var j = 0; j != cclass.Objects.Length; j++)
            {
                var obj = cclass.Objects[j];
                if (obj.Type() == type && obj.ObjectName() == objname)
                {
                    Obj = obj;
                    break;
                }
            }
            return Obj;
        }
        internal static CClass GetCClass(string cclassname)
        {
            CClass cclass = default;

            var cclasses = CClass.CClasses.Array.Span;
            for (var i = 0; i != cclasses.Length; i++)
            {
                var tcclass = cclasses[i];
                if (tcclass.Name == cclassname)
                {
                    cclass = tcclass;
                    break;
                }
            }
            return cclass;
        }
        /// <summary>
        /// Name of class.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Container of objects.
        /// </summary>
        internal ICClassObject[] Objects { get; private set; }
        /// <summary>
        /// Initialisating new CClass.
        /// </summary>
        /// <param name="Name"></param>
        internal CClass(string Name)
        {
            this.Name = Name;
            Objects = Array.Empty<ICClassObject>();
        }
        /// <summary>
        /// Replacing container of objects.
        /// </summary>
        /// <param name="objs"></param>
        internal void ReplaceContainer(ICClassObject[] objs)
        {
            Objects = objs;
        }
        /// <summary>
        /// Adding new object in container of objects.
        /// </summary>
        /// <param name="obj"></param>
        internal void AddObject(ICClassObject obj)
        {
            var objs = Objects.ToList();
            objs.Add(obj);
            Objects = objs.ToArray();
        }
        /*
        /// <summary>
        /// Get state of is dev class & normalize dev class (delete non dev objects).
        /// </summary>
        /// <param name="objs">Normalized container of cclass.</param>
        /// <returns></returns>
        internal bool IsDevClassAndNormalize(out ICClassObject[] objs)
        {
            bool isdev = false;
            var devlist = new List<ICClassObject>();
            foreach (var item in Objects)
            {
                if (item.IsDevScript())
                    devlist.Add(item);
            }
            if (devlist.Count > Objects.Length)
            {
                Objects = devlist.ToArray();
                isdev = true;
                foreach (var item in Objects)
                {
                    if (!item.IsDevScript())
                    {
                        item.SetCClass(null);
                    }
                }
            }
            ReplaceContainer(devlist.ToArray());
            objs = devlist.ToArray();
            return isdev;
        }
        */
        public override string ToString()
        {
            return $"{Name}";
        }
        /// <summary>
        /// Removing object from container of objects.
        /// </summary>
        /// <param name="obj"></param>
        internal void RemoveObject(ICClassObject obj)
        {
            var objs = Objects.ToList();
            foreach (var item in objs)
            {
                if (item == obj)
                    objs.Remove(item);
            }
            Objects = objs.ToArray();
        }
        /*
        /// <summary>
        /// Getting dev classes.
        /// </summary>
        /// <returns></returns>
        internal static CClass[] GetDevClasses()
        {
            var cclasses = new List<CClass>();
            foreach (var item in CClasses.Array.Span)
            {
                if (item.IsDevClassAndNormalize(out var objs))
                {
                    cclasses.Add(item);
                }
            }
            return cclasses.ToArray();
        }
        */
    }
}
