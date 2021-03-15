using DF.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using static DF.Classes.CallBackResult;

namespace DF.Classes
{
    internal sealed class CallBackSub<TCallBack> : ICallBackSub
    {
        /// <summary>
        /// Main callback.
        /// </summary>
        internal CallBack<TCallBack> CallBack { get; private set; }
        /// <summary>
        /// Function.
        /// </summary>
        internal CallBackAction<TCallBack> Action { get; private set; }
        public CallBackSub(CallBack<TCallBack> callBack, CallBackAction<TCallBack> action)
        {
            Action = action;
            CallBack = callBack;
        }
        /// <summary>
        /// Unsubscribe this subscription.
        /// </summary>
        internal void Unsubscribe()
        {
            CallBack.Unsubscribe(this);
        }

        #region ICallBackSub
        void ICallBackSub.Unsubscribe()
        {
            Unsubscribe();
        }
        #endregion
    }
}
