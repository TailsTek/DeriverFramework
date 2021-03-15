using System;
using System.Collections.Generic;
using System.Text;

namespace DF.Tools.Extensions
{
    public static class IntegerExtensions
    {
        public static int ToInt32(this string str)
        {
            return int.Parse(str);
        }
    }
}
