using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Debug
{
    public class LogResult
    {
        public ConsoleDebug.ActionResult Action;
        public LogEnums.ResultCode ResultCode;
        public Exception Exception;

        public LogResult(ConsoleDebug.ActionResult act, LogEnums.ResultCode rescode, Exception excp)
        {
            Action = act;
            ResultCode = rescode;
            Exception = excp;
        }
        public static LogResult Gen(ConsoleDebug.ActionResult act, LogEnums.ResultCode rescode, Exception excp)
        {
            return new LogResult(act, rescode, excp);
        }

        public override string ToString()
        {
            return $"Action: {Action.Method.Name} | Result Code: {ResultCode} | Exception: {Exception}";
        }
    }
}
