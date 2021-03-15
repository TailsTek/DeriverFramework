using DF.Classes;
using DF.Debug;
using DF.Enums;
using DF.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Tools
{
    public abstract class DFDevelop : ICClassObject
    {
        /// <summary>
        /// DevScript name.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Class of script.
        /// </summary>
        public CClass CClass { get; private set; }

        /// <summary>
        /// Init new DFDev class.
        /// </summary>
        /// <param name="name">Name of this class.</param>
        /// <param name="classname">Name of main class.</param>
        protected DFDevelop(string name, string classname)
        {
            AppStabilizer.Start += AppStabilizer_Start;
            AppStabilizer.Exit += AppStabilizer_Exit;
            AppStabilizer.UnException += AppStabilizer_UnException;
            Name = name;
            CClass.CheckCreateAdd(this, $"{classname}");
            AppStabilizer.Init();
        }

        private void AppStabilizer_UnException(EventArgs args)
        {
            if (!(CClass is null))
            {
                var arg = (UnhandledExceptionEventArgs)args;
                OnException(arg);
            }
        }

        private void AppStabilizer_Exit(EventArgs args)
        {
            if (!(CClass is null))
                OnExit();
        }

        private void AppStabilizer_Start(EventArgs args)
        {
            if (!(CClass is null))
                OnStart();
        }
        /// <summary>
        /// Invokes custom log.
        /// </summary>
        public virtual void InvokeLog(string text, LogEnums.ResultCode resultCode, ConsoleDebug.ActionResult action = null, ConsoleColor cColor = ConsoleColor.Green, Exception ex = null)
        {
            LogFramework.WriteNewLog(text, CClass, this, action, resultCode, cColor, ex);
        }
        /// <summary>
        /// Void on start.
        /// </summary>
        protected abstract void OnStart();
        /// <summary>
        /// Void on exit.
        /// </summary>
        protected abstract void OnExit();
        /// <summary>
        /// Void on unhandled exception.
        /// </summary>
        /// <param name="args"></param>
        protected abstract void OnException(UnhandledExceptionEventArgs args);

        string ICClassObject.ObjectName()
        {
            return Name;
        }

        CClassObjectType ICClassObject.Type()
        {
            return CClassObjectType.DevScript;
        }

        string ICClassObject.Invoke(params string[] parameters)
        {
            return "";
        }

        void ICClassObject.SetCClass(CClass cclass)
        {
            CClass = cclass;
        }

        object ICClassObject.GetObject()
        {
            return this;
        }

        int ICClassObject.GetHashCode()
        {
            return GetHashCode();
        }
    }
}
