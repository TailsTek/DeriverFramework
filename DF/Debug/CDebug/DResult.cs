using DF.Classes;
using DF.Enums;
using DF.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using static DF.Debug.ConsoleDebug;

namespace DF.Debug.CDebug
{
    public class DResult
    {
        public CDebugResult Result;
        public string Message;
        public ActionResult Action;
        public CClass CClass;
        public ICClassObject Object;
        public ConsoleColor CColor;
        public DResult(CDebugResult res, string mes, ActionResult action, CClass cClass, ICClassObject obj, ConsoleColor ccolor = ConsoleColor.Green)
        {
            Result = res;
            Message = mes;
            Action = action;
            CColor = ccolor;
            CClass = cClass;
            Object = obj;
        }
        public static DResult C(CDebugResult res, string mes, ActionResult action, CClass cClass, ICClassObject obj, ConsoleColor ccolor = ConsoleColor.Green)
        {
            return new DResult(res, mes, action, cClass, obj, ccolor);
        }
    }
}
