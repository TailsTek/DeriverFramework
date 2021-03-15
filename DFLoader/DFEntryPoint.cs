using DeriverFramework.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DFLoader
{
    public class DFEntryPoint
    {
        public static DynamicLibrary DF;
        public static bool OK { get; private set; }
        public static void Allocate(string Path)
        {
            DF = new DynamicLibrary(Path);
        }
        public static void Allocate(byte[] raw)
        {
            DF = new DynamicLibrary(raw);
        }
        public static void TestLoad(Exception ex)
        {
            if (!DF.Loaded)
            {
                Mes($"[{DateTime.Now.TimeOfDay}] [DFLoader] Failed load DeriverFramework!", ConsoleColor.Red);
                Mes("Press any key to exit...");
                Console.ReadKey();
                Environment.Exit(0);
            }
            else
            {
                Mes($"[{DateTime.Now.TimeOfDay}] [DFLoader] Code conflict error.", ConsoleColor.Red);
                Mes("---------------------------------------------------------------------------------------------------------------------------------", ConsoleColor.White);
                Mes(ex.ToString(), ConsoleColor.Red);
                Mes("---------------------------------------------------------------------------------------------------------------------------------", ConsoleColor.White);
                Mes("Press any key to exit...");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
        public static void AllocateTest()
        {
            var az = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var item in az)
            {
                var nm = item.GetName().Name;
                if (nm == "DeriverFramework" && !OK)
                    SetOK();
            }
        }
        private static void Mes(string mes, ConsoleColor cc = ConsoleColor.Gray)
        {
            var ccl = Console.ForegroundColor;
            Console.ForegroundColor = cc;
            Console.WriteLine(mes);
            Console.ForegroundColor = ccl;
        }
        public static void Parse()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            /*
            bool ok;
            if (!(DF is null) && DF.Raw != null)
            {
                try
                {
                    Assembly.Load(DF.Raw);
                }
                catch
                {
                    ok = false;
                }
                ok = true;
            }
            else
            {
                ok = false;
            }
            return ok;
            */
        }
        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            
            try
            {
                if (!DF.Loaded)
                {
                    string assembly = args.Name.Split(',')[0];

                    if (assembly == "DeriverFramework")
                    {
                        if (DF.Raw != null)
                        {
                            SetOK();
                            return Assembly.Load(DF.Raw);
                        }
                        else
                            return null;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }
        private static void SetOK()
        {
            DF.Loaded = true;
            OK = true;
            Mes($"[{DateTime.Now.TimeOfDay}] [DFLoader] {DF.Name} loaded.", ConsoleColor.Green);
        }
    }
}
