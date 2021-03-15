using DF.Classes;
using DF.Debug;
using DF.Debug.CDebug;
using DF.Enums;
using DF.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DF.Tools
{
    public sealed class ResourceManager : ICClassObject
    {
        internal CClass CClass;
        /// <summary>
        /// List of dynamic libs.
        /// </summary>
        public List<DynamicLibrary> DynamicLibraries { get; private set; }
        internal ResourceManager()
        {
            DynamicLibraries = new List<DynamicLibrary>();
            CClass.CheckCreateAdd(this, nameof(ResourceManager));
        }
        public static ResourceManager Create()
        {
            return new ResourceManager();
        }
        //public void AddDLibrary(string Name, string Ver, string Path)
        //{
        //    DynamicLibraries.Add(new DynamicLibrary(Name, Ver, Path));
        //}
        public void AddDLibrary(string Name, string Ver, byte[] raw)
        {
            DynamicLibraries.Add(new DynamicLibrary(Name, Ver, raw));
        }
        /// <summary>
        /// Load library lists.
        /// </summary>
        public DResult LoadAllLibraries(params CParameter[] paramms)
        {
            LogFramework.WriteNewLog($"Starting manager dynamic & static libraries", CClass, this, LoadAllLibraries, LogEnums.ResultCode.StartingMethod);
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

            var mes = $"Manager dynamic & static libraries enabled";
            return DResult.C(CDebugResult.Working, mes, LoadAllLibraries, CClass, this);
            /*
            LogFramework.WriteNewLog($"Loading dynamic libraries", LogEnums.Tools.ResourceManager, new Action(LoadAllLibraries), LogEnums.ResultCode.StartingMethod, null);
            try
            {
                foreach (DynamicLibrary item in DynamicLibraries)
                {
                    LogFramework.WriteNewLog($"Loading {item.Name} v{item.Ver}", LogEnums.Tools.ResourceManager, new Action(LoadAllLibraries), LogEnums.ResultCode.StartingInMethod, null);
                    try
                    {
                        if (!item.Load())
                            LogFramework.WriteNewLog($"Failed Load {item.Name} v{item.Ver}", LogEnums.Tools.ResourceManager, new Action(LoadAllLibraries), LogEnums.ResultCode.ErrorInMethod, null);
                    }
                    catch (Exception e)
                    {
                        LogFramework.WriteNewLog($"Failed Load {item.Name} v{item.Ver}", LogEnums.Tools.ResourceManager, new Action(LoadAllLibraries), LogEnums.ResultCode.ErrorInMethod, e);
                    }
                    finally
                    {
                        LogFramework.WriteNewLog($"Loaded {item.Name} v{item.Ver}", LogEnums.Tools.ResourceManager, new Action(LoadAllLibraries), LogEnums.ResultCode.OKInMethod, null);
                    }
                }
            }
            catch (Exception e)
            {
                LogFramework.WriteNewLog($"Failed load dynamic libraries", LogEnums.Tools.ResourceManager, new Action(LoadAllLibraries), LogEnums.ResultCode.ErrorMethod, null);
            }
            finally
            {
                LogFramework.WriteNewLog($"Dynamic libraries loaded", LogEnums.Tools.ResourceManager, new Action(LoadAllLibraries), LogEnums.ResultCode.OKMethod, null);
            }

            LogFramework.WriteNewLog($"Loading static libraries", LogEnums.Tools.ResourceManager, new Action(LoadAllLibraries), LogEnums.ResultCode.StartingMethod, null);
            try
            {
                foreach (var item in DynamicLibraries)
                {
                    LogFramework.WriteNewLog($"Loading {item.Name} v{item.Ver}", LogEnums.Tools.ResourceManager, new Action(LoadAllLibraries), LogEnums.ResultCode.StartingInMethod, null);
                    try
                    {
                        AppDomain.CurrentDomain.Load(item.Raw);
                    }
                    catch (Exception e)
                    {
                        LogFramework.WriteNewLog($"Failed Load {item.Name} v{item.Ver}", LogEnums.Tools.ResourceManager, new Action(LoadAllLibraries), LogEnums.ResultCode.ErrorInMethod, e);
                    }
                    finally
                    {
                        LogFramework.WriteNewLog($"Loaded {item.Name} v{item.Ver}", LogEnums.Tools.ResourceManager, new Action(LoadAllLibraries), LogEnums.ResultCode.OKInMethod, null);
                    }
                }
            }
            catch (Exception e)
            {
                LogFramework.WriteNewLog($"Failed load static libraries", LogEnums.Tools.ResourceManager, new Action(LoadAllLibraries), LogEnums.ResultCode.ErrorMethod, null);
            }
            finally
            {
                LogFramework.WriteNewLog($"Static libraries loaded", LogEnums.Tools.ResourceManager, new Action(LoadAllLibraries), LogEnums.ResultCode.OKMethod, null);
            }
            */
        }
        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                string assembly = args.Name.Split(',')[0];

                LogFramework.WriteNewLog($"Trying load {assembly}", CClass, this, LoadAllLibraries, LogEnums.ResultCode.StartingInMethod);

                if (CheckAssembly(assembly, out var assembl))
                {
                    LogFramework.WriteNewLog($"Loaded {assembly}", CClass, this, LoadAllLibraries, LogEnums.ResultCode.OKInMethod);
                    return Assembly.Load(assembl);
                }
                else if (assembly.EndsWith(".resources"))
                {
                    LogFramework.WriteNewLog($"Failed load : unknown type of {assembly}", CClass, this, LoadAllLibraries, LogEnums.ResultCode.WarnInMethod);
                    return null;
                }
                    
                else
                {
                    var excp = new Exception($"Сборка: \"{assembly}\" не может быть загружена.");
                    LogFramework.WriteNewLog($"Failed load {assembly}", CClass, this, LoadAllLibraries, LogEnums.ResultCode.ErrorInMethod, ConsoleColor.Red, excp);
                    throw excp;
                }
                    

                /*
                switch (assembly)
                {
                    case "OpenTK":
                        {
                            //MessageBox.Show("OpenTK loaded");
                            App.LibraryManager.SetResolvedLibrary($"{assembly}.dll");
                            App.LibraryManager.SetLoadedLibrary($"{assembly}.dll");
                            return Assembly.Load(App.LibraryManager.GetLibrary($"{assembly}.dll").Raw);
                        }
                    case "OpenTK.GLControl":
                        {
                            //MessageBox.Show("OpenTK.GLControl loaded");
                            App.LibraryManager.SetResolvedLibrary($"{assembly}.dll");
                            App.LibraryManager.SetLoadedLibrary($"{assembly}.dll");
                            return Assembly.Load(App.LibraryManager.GetLibrary($"{assembly}.dll").Raw);
                        }
                    case "System.Windows.Controls.DataVisualization.Toolkit":
                        {
                            //MessageBox.Show("System.Windows.Controls.DataVisualization.Toolkit loaded");
                            App.LibraryManager.SetResolvedLibrary($"{assembly}.dll");
                            App.LibraryManager.SetLoadedLibrary($"{assembly}.dll");
                            return Assembly.Load(App.LibraryManager.GetLibrary($"{assembly}.dll").Raw);
                        }


                    default:
                        
                }
                */
            }
            catch (Exception ex)
            {
                LogFramework.WriteNewLog($"Failed load lib", CClass, this, LoadAllLibraries, LogEnums.ResultCode.ErrorInMethod, ConsoleColor.Red, ex);
                return null;
            }
        }
        private bool CheckAssembly(string assemblyname, out byte[] data)
        {
            foreach (var item in DynamicLibraries)
            {
                if (item.Name == $"{assemblyname}.dll")
                {
                    data = item.Raw;
                    return true;
                }
            }
            data = null;
            return false;
        }

        string ICClassObject.ObjectName()
        {
            return nameof(ResourceManager);
        }

        CClassObjectType ICClassObject.Type()
        {
            return CClassObjectType.ResourceManager;
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
