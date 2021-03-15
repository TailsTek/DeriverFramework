using DF.Classes;
using DF.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using static DF.Classes.CallBackResult;

namespace DF.Tools
{
    public class CallBackManager
    {
        private CallBackHost Host;
        public CallBackManager(CallBackHost host)
        {
            Host = host;
        }
        public CallBackResult[] CallBack<T>(string name, T obj)
        {
            return Host.InvokeCallBack<T>(name, obj);
        }
        public ICallBackSub Subscribe<T>(string callbackname, CallBackAction<T> action)
        {
            return Host.Subscribe<T>(callbackname, action);
        }
        protected ICallBack AddCallBack<T>(CallBack<T> callBack)
        {
            return Host.AddCallBack(callBack);
        }
        protected void RemoveCallBack<T>(CallBack<T> callBack)
        {
            Host.RemoveCallBack(callBack);
        }
        protected void RemoveCallBackAt(int index)
        {
            Host.RemoveCallBackAt(index);
        }
    }
}
