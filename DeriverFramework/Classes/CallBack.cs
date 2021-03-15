using DF.Interfaces;
using DF.Structs;
using DF.Tools;
using System;
using System.Collections.Generic;
using System.Text;
using static DF.Classes.CallBackResult;

namespace DF.Classes
{
    public class CallBack<TCallBack> : ICallBack
    {
        /// <summary>
        /// Name of callback.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Callback host.
        /// </summary>
        internal CallBackHost CallBackHost { get; private set; }
        /// <summary>
        /// Subscriptions.
        /// </summary>
        internal ExtraArray<CallBackSub<TCallBack>> Subscriptions { get; private set; }
        public CallBack(string name)
        {
            Name = name;
            Subscriptions = new ExtraArray<CallBackSub<TCallBack>>(Array.Empty<CallBackSub<TCallBack>>());
        }
        /// <summary>
        /// Get as interface.
        /// </summary>
        /// <returns></returns>
        internal ICallBack GetICallBack()
        {
            return this;
        }
        /// <summary>
        /// Subscribe new subscription.
        /// </summary>
        /// <param name="func">Function.</param>
        /// <returns></returns>
        internal ICallBackSub Subscribe(CallBackAction<TCallBack> func)
        {
            CallBackSub<TCallBack> cbs = new CallBackSub<TCallBack>(this, func);
            foreach (var item in Subscriptions.Array.Span)
            {
                if (item == cbs)
                    throw new Exception("Now subscribed.");
            }
            Subscriptions.Add(cbs);
            return cbs;
        }
        /// <summary>
        /// Unsubscribe subscription.
        /// </summary>
        /// <param name="callBackSub"></param>
        internal void Unsubscribe(CallBackSub<TCallBack> callBackSub)
        {
            Subscriptions.Remove(callBackSub);
        }
        /// <summary>
        /// Remove callback.
        /// </summary>
        internal void Remove()
        {
            CallBackHost.RemoveCallBack(this);
        }
        /// <summary>
        /// Invoke for subscriptions.
        /// </summary>
        /// <param name="obj"></param>
        internal virtual CallBackResult[] InvokeAll(TCallBack obj)
        {
            var results = new List<CallBackResult>();
            foreach (var sub in Subscriptions.Array.Span)
            {
                results.Add(sub.Action.Invoke(obj));
            }
            return results.ToArray();
        }
        #region ICallBack
        string ICallBack.Name()
        {
            return Name;
        }
        CallBackResult[] ICallBack.Invoke(object obj)
        {
            return InvokeAll((TCallBack)obj);
        }
        void ICallBack.SetCallBackHost(CallBackHost callBackHost)
        {
            CallBackHost = callBackHost;
        }
        void ICallBack.Remove()
        {
            Remove();
        }

        ICallBackSub ICallBack.Subscribe(object func)
        {
            return Subscribe((CallBackAction<TCallBack>)func);
        }
        #endregion
    }
}
