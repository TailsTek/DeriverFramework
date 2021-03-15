using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Debug
{
    public class LogEnums
    {
        public enum Tools : byte
        {
            ResourceManager,
            DFDevelop,
            AppStabilizer,
            ConsoleDebug,
            PackageManager,
        }
        public enum ResultCode : byte
        {
            Waiting,
            Warn,
            Error,

            StartingMethod,
            WorkingMethod,
            OKMethod,
            ErrorMethod,
            WarnMethod,

            StartingInMethod,
            WorkingInMethod,
            OKInMethod,
            ErrorInMethod,
            WarnInMethod,
        }
    }
}
