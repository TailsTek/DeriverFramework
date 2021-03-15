using DF.Classes;
using DF.Debug.CDebug;
using DF.Enums;
using DF.Interfaces;
using DF.Structs;
using DF.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DF.Debug
{
    public class ConsoleDebug : ICClassObject
    {
        public delegate DResult ActionResult(params CParameter[] paramms);
        internal static FastConsoleWriter.Console FConsole;
        internal static void WriteLine(string text, ConsoleColor cc = ConsoleColor.Gray)
        {
            if (FConsole is null)
            {
                FConsole = new FastConsoleWriter.Console(Console.Out);
                FConsole.Start();
            }
            FConsole.WriteLine(text, cc);
        }

        internal static CClass CClass;
        internal static ConsoleDebug Instance;
        public static void Alloc()
        {
            In.Tools.AllocConsole();
        }
        internal const string CMD = "CMD";
        internal const string CMDAPI = "CMDAPI";
        internal static CCommand[] GetCCommandList()
        {
            var cclass = CClass.GetCClass(CMD);
            var objs = cclass.Objects;
            CCommand[] cmds = new CCommand[objs.Length];
            for (var i = 0; i != cmds.Length; i++)
            {
                cmds[i] = (CCommand)objs[i];
            }
            return cmds;
        }
        public ConsoleDebug()
        {
            

            //Commands = new ExtraArray<CCommand>(Array.Empty<CCommand>());

            CCommand.SNew(CommandActions.Help, "help", "Default", true, "Showing command list or using as command helper", "Command");
            CCommand.SNew(CommandActions.Exit, "exit", "Default", false, "Stopping current process");
            CCommand.SNew(CommandActions.DebugBeep, "beep", "Default", true, "Debug beep, returns value", "State");
            CCommand.SNew(CommandActions.ClearConsole, "clear", "Default", false, "Clears console bufer");

            CCommand.SNew(CommandActions.PackFileInPkg, "pkgpack", "PackageManager", false, "Packing file into package", "File path", "PKG name");
            CCommand.SNew(CommandActions.UnpackFileInPkg, "pkgunpack", "PackageManager", false, "Unpacking file into folder", "Filename", "PKG name", "Folder path");
            CCommand.SNew(CommandActions.CreatePKG, "pkgcreate", "PackageManager", false, "Creating new pkg in pkgmgr", "PKG name");
            CCommand.SNew(CommandActions.ImportPKG, "pkgimport", "PackageManager", false, "Importing pkg from folder", "File path");
            CCommand.SNew(CommandActions.ExportPKG, "pkgexport", "PackageManager", false, "Exporting pkg into folder", "Folder path", "PKG name");
            Instance = this;
            CClass.CheckCreateAdd(this, nameof(ConsoleDebug));
        }
        internal static bool BeepSound { get; set; } = false;
        internal static ExtraArray<CParameter> Params { get; private set; } = new ExtraArray<CParameter>(new Memory<CParameter>(Array.Empty<CParameter>()));
        /*
        internal void Add(ConsoleDebug.ActionResult action, string cline, string cl, bool uwp = false, string description = null)
        {
            Commands.Add(CCommand.New(action, cline, cl, uwp, description));
        }
        internal void SAdd(ConsoleDebug.ActionResult action, string cline, string cl, bool uwp = false, string description = null)
        {
            Commands.Add(CCommand.SNew(action, cline, cl, uwp, description));
        }
        */
        public void WaitCommand(string cline)
        {
            bool normal = true;
            if (string.IsNullOrEmpty(cline))
                normal = false;

            if (normal)
            {
                if (cline[0] == ' ')
                {
                    for (var i = 1; i != cline.Length; i++)
                    {
                        if (cline[i] != ' ')
                        {
                            cline = cline.Remove(0, i);
                            break;
                        }
                    }
                }
                if (string.IsNullOrEmpty(cline))
                    normal = false;
            }

            bool finded = false;

            if (normal)
            {
                string clinesp = cline;
                //var spacep = clinesp.Length - 1;
                for (var i = clinesp.Length - 1; i != 0; i--)
                {
                    if (clinesp[i] == ' ')
                        clinesp = clinesp.Remove(i, 1);
                    else
                        break;
                }
                string c = clinesp;
                var clines = CommandActions.GetLine(clinesp);
                if (!Params.IsNull())
                    Params.Clear();
                if (clines.Length > 1)
                {
                    Params = new ExtraArray<CParameter>(CParameter.FromLine(clinesp).ToArray().AsMemory());

                    var cc = clinesp.Split(' ');
                    c = cc[0];
                }
                foreach (var item in GetCCommandList())
                {
                    if (item.CommandLine == c)
                    {
                        finded = true;
                        if (Params.Count == item.Params.Length)
                        {
                            InvokeAction(item);
                        }
                        else
                        {
                            if (item.Params.Length > 0 && !item.UseWithoutParams)
                            {
                                var ot = $"";
                                foreach (var item1 in item.Params)
                                {
                                    ot += $" <{item1}>";
                                }
                                Beep(800, 100);
                                LogFramework.WriteNewLog($"Instruction of {item.CommandLine}:{ot}.", CClass, this, null, LogEnums.ResultCode.WarnMethod);
                            }
                            else if (item.Params.Length > 0 && item.UseWithoutParams)
                            {
                                InvokeAction(item);
                            }
                            else
                            {
                                Beep(800, 100);
                                LogFramework.WriteNewLog($"{item.CommandLine}: using without parameters.", CClass, this, null, LogEnums.ResultCode.WarnMethod);
                            }
                        }
                    }
                }
            }
            if (!finded)
            {
                LogFramework.WriteNewLog("Unknown command.", CClass, this, null, LogEnums.ResultCode.ErrorMethod);
                Beep(85, 300);
            }
        }

        private void InvokeAction(CCommand cmd)
        {
            var dres = cmd.InvokeAction(Params.Array.ToArray());
            if (!string.IsNullOrEmpty(dres.Message))
            {
                if (dres.Result == CDebugResult.Warn)
                {
                    LogFramework.WriteNewLog(dres.Message, dres.CClass, dres.Object, dres.Action, LogEnums.ResultCode.WarnMethod, dres.CColor);
                    Beep(800, 100);
                }
                else if (dres.Result == CDebugResult.Error)
                {
                    LogFramework.WriteNewLog(dres.Message, dres.CClass, dres.Object, dres.Action, LogEnums.ResultCode.ErrorMethod, dres.CColor);
                    Beep(85, 300);
                }
                else if (dres.Result == CDebugResult.OK)
                {
                    LogFramework.WriteNewLog(dres.Message, dres.CClass, dres.Object, dres.Action, LogEnums.ResultCode.OKMethod, dres.CColor);
                    Beep(3500, 100);
                }
                else if (dres.Result == CDebugResult.Working)
                {
                    LogFramework.WriteNewLog(dres.Message, dres.CClass, dres.Object, dres.Action, LogEnums.ResultCode.WorkingMethod, dres.CColor);
                }
            }
        }

        private void Beep(int freq, int dur)
        {
            if (BeepSound)
                Console.Beep(freq, dur);
        }

        string ICClassObject.ObjectName()
        {
            return nameof(ConsoleDebug);
        }

        CClassObjectType ICClassObject.Type()
        {
            return CClassObjectType.ConsoleDebug;
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
    public class ConsoleAPI : ICClassObject
    {
        internal CClass CClass;
        /// <summary>
        /// CMD.
        /// </summary>
        public CCommand Command { get; private set; }
        /// <summary>
        /// Maximum of params.
        /// </summary>
        public int MaxParams { get; private set; }
        /// <summary>
        /// Invokes message for debug.
        /// </summary>
        /// <param name="text">Text for invoke.</param>
        /// <param name="resultCode">State of cmd.</param>
        /// <param name="cColor">Console color (work if ResultCode = OK)</param>
        public void InvokeMessage(string text, LogEnums.ResultCode resultCode, ConsoleColor cColor = ConsoleColor.Green, Exception ex = null)
        {
            LogFramework.WriteNewLog(text, CClass, this, Command.Action, resultCode, cColor, ex);
        }

        string ICClassObject.ObjectName()
        {
            return Command.CommandLine;
        }

        CClassObjectType ICClassObject.Type()
        {
            return CClassObjectType.CMDAPI;
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

        /// <summary>
        /// Getting params for api.
        /// </summary>
        public ExtraArray<CParameter> Params
        {
            get
            {
                return new ExtraArray<CParameter>(ConsoleDebug.Params.Array);
            }
        }
        public ConsoleAPI(CCommand cmd)
        {
            MaxParams = 0;
            if (cmd.Params != null && cmd.Params.Length > 0)
            {
                MaxParams = cmd.Params.Length;
            }
            //ConsoleDebug.Commands.Add(cmd);
            Command = cmd;
            CClass.CheckCreateAdd(this, ConsoleDebug.CMDAPI);
        }
    }
    public class ConsoleDebugThread
    {
        public static ConsoleDebug ConsoleDebug;
        public Thread Thread;
        public void Start()
        {
            Thread = new Thread(new ThreadStart(Threadstart))
            {
                Name = nameof(ConsoleDebugThread),
                Priority = ThreadPriority.Normal,
            };
            Thread.SetApartmentState(ApartmentState.STA);
            Thread.Start();
        }
        public void Wait()
        {
            while (ConsoleDebug is null)
            {
                Thread.Sleep(1);
            }
        }
        private void Threadstart()
        {
            ConsoleDebug = new ConsoleDebug();
            while (true)
            {
                Thread.Sleep(1);
                ConsoleDebug.WaitCommand(Console.ReadLine());
            }
        }
    }
    public class CCommand : ICClassObject
    {
        internal CClass CClass;
        /// <summary>
        /// Actions of cmd.
        /// </summary>
        public ConsoleDebug.ActionResult Action { get; private set; }
        /// <summary>
        /// CommandLine name.
        /// </summary>
        public string CommandLine { get; private set; }
        /// <summary>
        /// Description of cmd.
        /// </summary>
        public string Description { get; private set; }
        /// <summary>
        /// State of system cmd.
        /// </summary>
        internal bool System { get; private set; }
        /// <summary>
        /// Params of cmd.
        /// </summary>
        public string[] Params { get; private set; }
        /// <summary>
        /// Class of cmd.
        /// </summary>
        public string Class { get; private set; }
        /// <summary>
        /// Cmd can uses without params.
        /// </summary>
        public bool UseWithoutParams { get; private set; }
        /// <summary>
        /// Overrideable cmd.
        /// </summary>
        internal CCommand OverrideCMD { get; private set; }
        /// <summary>
        /// Invokes actions.
        /// </summary>
        internal DResult InvokeAction(params CParameter[] paramms)
        {
            return Action.Invoke(paramms);
        }
        /// <summary>
        /// Creating new cmd.
        /// </summary>
        /// <param name="action">Action for cmd.</param>
        /// <param name="cline">CommandLine.</param>
        /// <param name="cl">Class</param>
        /// <param name="uwp">Using without params.</param>
        /// <param name="description">Description of cmd.</param>
        /// <param name="sys">Is system.</param>
        /// <param name="parameters">Params of cmd.</param>
        internal CCommand(ConsoleDebug.ActionResult action, string cline, string cl, bool uwp, string description = null, bool sys = true, string[] parameters = null)
        {
            Action = action;
            CommandLine = cline;
            System = sys;
            Params = parameters;
            Class = cl;
            UseWithoutParams = uwp;

            if (description == null)
                Description = "Unknown command description!";
            else
                Description = description;
            CClass.CheckCreateAdd(this, ConsoleDebug.CMD);
        }
        public static CCommand New(ConsoleDebug.ActionResult action, string cline, string cl, bool usewithoutparams, string description = null, params string[] parameter)
        {
            return new CCommand(action, cline, cl, usewithoutparams, description, false, parameter);
        }
        internal static CCommand SNew(ConsoleDebug.ActionResult action, string cline, string cl, bool usewithoutparams, string description = null, params string[] parameter)
        {
            return new CCommand(action, cline, cl, usewithoutparams, description, true, parameter);
        }

        string ICClassObject.ObjectName()
        {
            return CommandLine;
        }

        CClassObjectType ICClassObject.Type()
        {
            return CClassObjectType.CMD;
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
    public class CParameter
    {
        public string Name;
        public Type TypeParameter
        {
            get
            {
                return Value.GetType();
            }
        }
        public object Value;

        public CParameter(string name, object value)
        {
            Name = name;
            Value = value;
        }
        public override string ToString()
        {
            return $"{Name} {Value}";
        }
        public static CParameter NewLine(string line, string name)
        {
            return new CParameter(name, line);
        }
        public static CParameter[] FromLine(string line, int max)
        {
            var lines = CommandActions.GetLine(line).ToList();
            lines.RemoveAt(0);
            List<CParameter> parames = new List<CParameter>();
            for (var i = 0; i != max; i++)
            {
                parames.Add(new CParameter("", lines[i]));
            }
            return parames.ToArray();
        }
        public static CParameter[] FromLine(string line)
        {
            var lines = CommandActions.GetLine(line).ToList();
            lines.RemoveAt(0);
            List<CParameter> parames = new List<CParameter>();
            if (lines.Count > 1)
            {
                for (var i = 0; i != lines.Count; i++)
                {
                    parames.Add(new CParameter("", lines[i]));
                }
            }
            else
            {
                parames.Add(new CParameter("", lines[0]));
            }
            return parames.ToArray();
        }
    }
    internal class CommandActions
    {
        internal static void Mes(string mes = "", ConsoleColor cc = ConsoleColor.Gray)
        {
            //var ccl = Console.ForegroundColor;
            //Console.ForegroundColor = cc;
            ConsoleDebug.WriteLine($"{mes}", cc);
            //Console.ForegroundColor = ccl;
        }
        public static DResult Exit(params CParameter[] Params)
        {
            var mes = "Exiting...";
            Environment.Exit(0);
            return DResult.C(CDebugResult.OK, mes, Exit, ConsoleDebug.CClass, ConsoleDebug.Instance);
        }
        public static DResult DebugBeep(params CParameter[] Params)
        {
            if (Params.Length > 0)
            {
                var s = Params[0].Value.ToString();
                if (s == "true" || s == "t" || s == "1")
                {
                    ConsoleDebug.BeepSound = true;

                    var mes = "Console Debug beep is enabled.";
                    return DResult.C(CDebugResult.OK, mes, DebugBeep, ConsoleDebug.CClass, ConsoleDebug.Instance, ConsoleColor.Green);
                }
                else if (s == "false" || s == "f" || s == "0")
                {
                    ConsoleDebug.BeepSound = false;

                    var mes = "Console Debug beep disabled.";
                    return DResult.C(CDebugResult.OK, mes, DebugBeep, ConsoleDebug.CClass, ConsoleDebug.Instance, ConsoleColor.DarkRed);
                }
                else
                {
                    var mes = "Unknown parameter.";
                    return DResult.C(CDebugResult.Warn, mes, DebugBeep, ConsoleDebug.CClass, ConsoleDebug.Instance);
                }
            }
            else
            {
                var ok = "";
                if (ConsoleDebug.BeepSound)
                    ok = "enabled";
                else
                    ok = "disabled";

                var mes = $"Console Debug beep {ok}.";
                return DResult.C(CDebugResult.OK, mes, DebugBeep, ConsoleDebug.CClass, ConsoleDebug.Instance, ConsoleColor.White);
            }
            /*
            if (!ConsoleDebug.BeepSound)
            {
                LogFramework.WriteNewLog("Console Debug beep enabled.", LogEnums.Tools.ConsoleDebug, DebugBeep, LogEnums.ResultCode.OKMethod, ConsoleColor.Green);
                ConsoleDebug.BeepSound = true;
            }
            else
            {
                LogFramework.WriteNewLog("Console Debug beep disabled.", LogEnums.Tools.ConsoleDebug, DebugBeep, LogEnums.ResultCode.OKMethod, ConsoleColor.DarkRed);
                ConsoleDebug.BeepSound = false;
            } 
            */
        }
        public static DResult ClearConsole(params CParameter[] Params)
        {
            Console.Clear();
            var mes = "Console buffer cleared.";
            return DResult.C(CDebugResult.OK, mes, ClearConsole, ConsoleDebug.CClass, ConsoleDebug.Instance);
        }
        public static DResult Help(params CParameter[] Params)
        {
            if (Params.Length > 0)
            {
                var param = Params[0].Value.ToString();
                /*
                for (var i = 0; i != param.Length; i++)
                {
                    var par = param;
                    if (par[i] == ' ')
                        param = par.Remove(i, 1);
                }
                */
                bool finded = false;
                CCommand cc = null;
                foreach (var item in ConsoleDebug.GetCCommandList())
                {
                    if (item.CommandLine == param)
                    {
                        finded = true;
                        cc = item;
                        break;
                    }
                }
                if (finded)
                {
                    var ot = "";
                    var s = "";
                    if (cc.System)
                        s = "System";
                    else
                        s = "Custom";
                    ot += $"{s} : {cc.CommandLine}";
                    foreach (var item in cc.Params)
                    {
                        ot += $" <{item}>";
                    }
                    ot += $" - {cc.Description}.";
                    return DResult.C(CDebugResult.OK, ot, Help, ConsoleDebug.CClass, ConsoleDebug.Instance);
                }
                else
                {
                    var mes = "Unknown command in help list.";
                    return DResult.C(CDebugResult.Warn, mes, Help, ConsoleDebug.CClass, ConsoleDebug.Instance);
                }
            }
            else
            {
                string ot = "Command list:\n";
                var cll = new List<String>();
                var cs = ConsoleDebug.GetCCommandList();
                for (var j = 0; j != cs.Length; j++)
                {
                    var cl = cs[j].Class;
                    if (cll.Contains(cl))
                    {
                        continue;
                    }
                    for (var i = 0; i != cs.Length; i++)
                    {
                        if (cs[i].Class == cl)
                        {
                            var s = "";
                            if (cs[i].System)
                                s = "System";
                            else
                                s = "Custom";
                            string val = $"  {s} : {cs[i].Class} : {cs[i].CommandLine} - {cs[i].Description}";
                            if (i - 1 != cs.Length)
                                ot += "\n";
                            ot += val;
                        }
                    }
                    if (j - 1 != cs.Length)
                        ot += "\n";
                    cll.Add(cl);
                }
                return DResult.C(CDebugResult.OK, ot, Help, ConsoleDebug.CClass, ConsoleDebug.Instance, ConsoleColor.White);
            }
        }
        public static DResult CreatePKG(params CParameter[] Params)
        {
            var pkgname = ConsoleDebug.Params[0].Value.ToString();
            var pp = new ProtectedPackage
            {
                Name = pkgname
            };
            PackageManager.Instance.AddPkg(pp);
            return DResult.C(CDebugResult.OK, "", CreatePKG, ConsoleDebug.CClass, ConsoleDebug.Instance);
        }
        public static DResult ImportPKG(params CParameter[] Params)
        {
            var filepath = ConsoleDebug.Params[0].Value.ToString();
            PackageManager.Instance.Import(filepath);
            return DResult.C(CDebugResult.OK, "", ImportPKG, ConsoleDebug.CClass, ConsoleDebug.Instance);
        }
        public static DResult ExportPKG(params CParameter[] Params)
        {
            var folderpath = ConsoleDebug.Params[0].Value.ToString();
            var pkgname = ConsoleDebug.Params[1].Value.ToString();
            PackageManager.Instance.Export(pkgname, folderpath);
            return DResult.C(CDebugResult.OK, "", ExportPKG, ConsoleDebug.CClass, ConsoleDebug.Instance);
        }
        public static DResult PackFileInPkg(params CParameter[] Params)
        {
            var fpath = ConsoleDebug.Params[0].Value.ToString();
            var pkgname = ConsoleDebug.Params[1].Value.ToString();
            var sp = Source.Pack(fpath);
            PackageManager.Instance.AddToPkg(pkgname, sp);
            return DResult.C(CDebugResult.OK, "", PackFileInPkg, ConsoleDebug.CClass, ConsoleDebug.Instance);
        }
        public static DResult UnpackFileInPkg(params CParameter[] Params)
        {
            var filename = ConsoleDebug.Params[0].Value.ToString();
            var pkgname = ConsoleDebug.Params[1].Value.ToString();
            var folderpath = ConsoleDebug.Params[2].Value.ToString();
            var pp = PackageManager.Instance.GetPkg(pkgname);
            pp.Unpack(filename, folderpath);
            return DResult.C(CDebugResult.OK, "", UnpackFileInPkg, ConsoleDebug.CClass, ConsoleDebug.Instance);
        }
        public static string[] GetLine(string line)
        {
            return line.Split(' ');
        }
    }
}
