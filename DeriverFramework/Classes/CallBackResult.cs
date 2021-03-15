using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DF.Classes
{
    public class CallBackResult
    {
        public delegate CallBackResult CallBackAction<T>(T obj);
        public List<object> Values { get; private set; }

        public CallBackResult(params object[] values)
        {
            Values = values.ToList();
        }
    }
}
