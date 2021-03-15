using System;
using System.Collections.Generic;
using System.Text;

namespace DF.Tools.Extensions
{
    public static class ULongExtensions
    {
        public static ulong ToULong(this string str)
        {
            return ulong.Parse(str);
        }
    }
}
