using System;
using System.Collections.Generic;
using System.Text;

namespace DF.Debug.CDebug
{
    public struct DBool
    {
        public DBool(string val)
        {
            if (True.Contains(val))
                Value = val;
            else if (False.Contains(val))
                Value = val;
            else
            {
                Console.WriteLine('"' + val + '"' + " replaced to False.");
                Value = "f";
            }
        }
        private string Value;
        private static List<string> True = new List<string>() 
        {
            "True", "TRUE", "true", "T", "t", "1",
        };
        private static List<string> False = new List<string>()
        {
            "False", "FALSE", "false", "F", "f", "0",
        };
        public bool GetBool(string st)
        {
            bool state = false;
            if (True.Contains(st))
                state = true;
            else if (False.Contains(st))
                state = false;
            else
                state = default;
            return state;
        }
    }
}
