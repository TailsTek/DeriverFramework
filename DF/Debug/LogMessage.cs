using DF.Classes;
using DF.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Debug
{
    public class LogMessage
    {
        public string Message;
        public string Time;
        public CClass CClass;
        public ICClassObject Object;
        public LogResult Result;

        public static LogMessage Create(string Mes, CClass cclass, ICClassObject obj, ConsoleDebug.ActionResult act, LogEnums.ResultCode rescode, Exception excp)
        {
            var res = LogResult.Gen(act, rescode, excp);
            return new LogMessage() { Message = Mes, CClass = cclass, Object = obj, Time = DateTime.Now.ToString("hh:mm:ss"), Result = res};
        }

        public override string ToString()
        {
            return $"Mes: {Message} | Time: {Time} | Class: {CClass.Name} | {Result}";
        }

        public string NormalString()
        {
            var excp = "";
            if (Result.Exception != null)
                excp = Result.Exception.Message;
            var state = "";
            if (Result.ResultCode == LogEnums.ResultCode.Error || Result.ResultCode == LogEnums.ResultCode.ErrorInMethod || Result.ResultCode == LogEnums.ResultCode.ErrorMethod)
                state = " [ERROR]";
            else if (Result.ResultCode == LogEnums.ResultCode.Warn || Result.ResultCode == LogEnums.ResultCode.WarnInMethod || Result.ResultCode == LogEnums.ResultCode.WarnMethod)
                state = " [WARN]";
            else if (Result.ResultCode == LogEnums.ResultCode.OKInMethod || Result.ResultCode == LogEnums.ResultCode.OKMethod)
                state = "";

            var objname = Object.ObjectName();

            var var1 = $"[{Time}]{state} [{CClass.Name}]: {Message}";
            var var2 = $"[{Time}]{state} [{CClass.Name}] [{objname}]: {Message}";

            var res = "";

            if (CClass.Name == objname)
                res = var1;
            else
                res = var2;

            return res;
        }
    }
}
