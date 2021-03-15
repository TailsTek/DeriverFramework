using DF.Classes;
using DF.Tools;
using System;
using System.Collections.Generic;
using System.Text;
using static DF.Classes.CallBackResult;

namespace DF.Interfaces
{
    public interface ICallBack
    {
        public string Name();
        internal CallBackResult[] Invoke(object obj);
        internal void SetCallBackHost(CallBackHost callBackHost);
        public void Remove();
        public ICallBackSub Subscribe(object func);
    }
}
