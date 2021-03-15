using DF.Classes;
using DF.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static DF.Classes.CallBackResult;

namespace DF.Tools
{
    public abstract class CallBackHost
    {
        /// <summary>
        /// All callbacks.
        /// </summary>
        internal List<ICallBack> CallBacks { get; private set; }
        protected CallBackHost(params ICallBack[] callbacks)
        {
            CallBacks = callbacks.ToList();
        }
        protected CallBackHost()
        {
            CallBacks = new List<ICallBack>();
        }
        /// <summary>
        /// Add callback.
        /// </summary>
        /// <param name="callBack"></param>
        /// <returns></returns>
        internal ICallBack AddCallBack(ICallBack callBack)
        {
            callBack.SetCallBackHost(this);
            CallBacks.Add(callBack);
            return callBack;
        }
        /// <summary>
        /// Removing callback.
        /// </summary>
        /// <param name="callBack"></param>
        internal void RemoveCallBack(ICallBack callBack)
        {
            CallBacks.Remove(callBack);
        }
        /// <summary>
        /// Removing callback by index.
        /// </summary>
        /// <param name="index"></param>
        internal void RemoveCallBackAt(int index)
        {
            CallBacks.RemoveAt(index);
        }
        /// <summary>
        /// Subscribe new subscription.
        /// </summary>
        /// <typeparam name="TCallBack"></typeparam>
        /// <param name="callbackname"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        internal ICallBackSub Subscribe<TCallBack>(string callbackname, CallBackAction<TCallBack> action)
        {
            CallBack<TCallBack> callback = default;
            foreach (var cb in CallBacks)
            {
                if (cb.Name() == callbackname)
                {
                    callback = (CallBack<TCallBack>)cb;
                    break;
                }
            }
            if (callback is null)
                throw new Exception($"Unknown callback: {callbackname}");
            return callback.Subscribe(action);
        }
        /// <summary>
        /// Invoke callback.
        /// </summary>
        /// <typeparam name="TCallBack"></typeparam>
        /// <param name="callbackname"></param>
        /// <param name="obj"></param>
        internal virtual CallBackResult[] InvokeCallBack<TCallBack>(string callbackname, TCallBack obj)
        {
            ICallBack callback = default;
            foreach (var cb in CallBacks)
            {
                if (cb.Name() == callbackname)
                {
                    callback = cb; 
                    break;
                }
            }
            if (callback is null)
                throw new Exception($"Unknown callback: {callbackname}");
            return callback.Invoke(obj);
        }
    }
}
