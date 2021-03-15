using DF.Classes;
using DF.Interfaces;
using DF.Structs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Debug
{
    public class LogFramework
    {
        internal static ExtraArray<LogMessage> Logs { get; private set; } = new ExtraArray<LogMessage>(Array.Empty<LogMessage>());
        internal static void SaveLogs()
        {
            var time = DateTime.Now.ToString("yyyy_MM_dd-hh_mm_ss");

            var log = "";

            foreach (var item in Logs.Array.Span)
            {
                log += item.NormalString() + "\n";
            }

            if (!Directory.Exists($@"{AppDomain.CurrentDomain.BaseDirectory}\logs"))
            {
                Directory.CreateDirectory($@"{AppDomain.CurrentDomain.BaseDirectory}\logs");
            }

            var path = @$"{AppDomain.CurrentDomain.BaseDirectory}logs\{time}.txt";
            using (var file = File.CreateText(path))
            {
                file.Write(log);
            }
        }
        public override string ToString()
        {
            var ss = "";
            foreach (var item in Logs.Array.Span)
            {
                ss += $"{item} \n";
            }
            return ss;
        }
        internal static void WriteNewLog(string Mes, CClass cclass, ICClassObject obj, ConsoleDebug.ActionResult act, LogEnums.ResultCode rescode, ConsoleColor cc = ConsoleColor.Green, Exception excp = null)
        {
            var log = LogMessage.Create(Mes, cclass, obj, act, rescode, excp);
            Logs.Add(log);
            
            if (rescode == LogEnums.ResultCode.ErrorInMethod || rescode == LogEnums.ResultCode.ErrorMethod || rescode == LogEnums.ResultCode.Error)
            {
                CommandActions.Mes(log.NormalString(), ConsoleColor.Red);
            }
            if (rescode == LogEnums.ResultCode.WarnInMethod || rescode == LogEnums.ResultCode.WarnMethod || rescode == LogEnums.ResultCode.Warn)
            {
                CommandActions.Mes(log.NormalString(), ConsoleColor.Yellow);
            }
            if (rescode == LogEnums.ResultCode.WorkingInMethod || rescode == LogEnums.ResultCode.WorkingMethod)
            {
                CommandActions.Mes(log.NormalString(), ConsoleColor.Cyan);
            }
            if (rescode == LogEnums.ResultCode.StartingInMethod || rescode == LogEnums.ResultCode.StartingMethod)
            {
                CommandActions.Mes(log.NormalString(), ConsoleColor.Blue);
            }
            if (rescode == LogEnums.ResultCode.OKInMethod || rescode == LogEnums.ResultCode.OKMethod)
            {
                if (cc == ConsoleColor.Green)
                    CommandActions.Mes(log.NormalString(), ConsoleColor.Green);
                else
                    CommandActions.Mes(log.NormalString(), cc);
            }
        }
    }
}
