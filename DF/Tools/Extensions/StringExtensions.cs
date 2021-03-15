using System;
using System.Collections.Generic;
using System.Text;

namespace DF.Tools.Extensions
{
    public static class StringExtensions
    {
        public static string CloneString(this string str)
        {
            return new string(str.ToCharArray());
        }
    }
}
