using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTools.Utils
{
    public static class Compiller
    {
        /*
        internal static IJitCompiller JitCompiller;
        internal static string[] Assemblies = new string[] { "netstandard.dll", "DTools.dll" };
        public static void InsertCompiller(IJitCompiller compiller)
        {
            JitCompiller = compiller;
        }
        public static T Compile<T>(string code, params string[] usings)
        {
            var list = usings.ToList();
            list.Add("System");
            usings = list.ToArray();
            return JitMethod<T>.FastCompile<T>(code, JitCompiller, Assemblies, usings);
        }
        */
    }
}
