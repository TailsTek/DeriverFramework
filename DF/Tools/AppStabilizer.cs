using DF.Classes;
using DF.Debug;
using DF.Enums;
using DF.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Tools
{
    public sealed class AppStabilizer : ICClassObject
    {
        public delegate void AppHandler(EventArgs args);
        public static event AppHandler Start;
        public static event AppHandler UnException;
        public static event AppHandler Exit;
        internal static Stopwatch MeasureSpeed;
        internal static double StartedTimeMs;
        internal static double StoppedTimeMs;

        private EventHandler ProcessExit = CurrentDomain_ProcessExit;
        private UnhandledExceptionEventHandler UnhandledException = CurrentDomain_UnhandledException;

        /// <summary>
        /// Class of AppStabilizer.
        /// </summary>
        internal static AppStabilizer Instance;
        internal static CClass CClass;
        public AppStabilizer()
        {
            CClass.CheckCreateAdd(this, nameof(AppStabilizer));
            LogFramework.WriteNewLog($"Welcome!", CClass, this, null, LogEnums.ResultCode.StartingMethod);
            MeasureSpeed = Stopwatch.StartNew();
            AppDomain.CurrentDomain.ProcessExit += ProcessExit;
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
            Start?.Invoke(new EventArgs());

            StartedTimeMs = MeasureSpeed.Elapsed.TotalMilliseconds;
            MeasureSpeed.Stop();
            LogFramework.WriteNewLog($"Started for {StartedTimeMs} ms.", CClass, this, null, LogEnums.ResultCode.OKMethod);
        }

        public static void Init()
        {
            if (AppStabilizer.Instance is null)
                AppStabilizer.Instance = new AppStabilizer();
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            MeasureSpeed = Stopwatch.StartNew();
            Exit?.Invoke(e);
            StoppedTimeMs = MeasureSpeed.Elapsed.TotalMilliseconds;
            MeasureSpeed.Stop();
            LogFramework.WriteNewLog($"Good bye! (ended for {StoppedTimeMs} ms.)", CClass, Instance, null, LogEnums.ResultCode.OKMethod);
            LogFramework.SaveLogs();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogFramework.WriteNewLog($"Critical error: {((Exception)(e.ExceptionObject)).Message}", CClass, Instance, null, LogEnums.ResultCode.Error, ConsoleColor.Green, (Exception)e.ExceptionObject);
            //CommandActions.Mes($"{((Exception)(e.ExceptionObject)).Message}", ConsoleColor.Red);
            UnException?.Invoke(e);
            LogFramework.SaveLogs();
        }

        string ICClassObject.ObjectName()
        {
            return nameof(AppStabilizer);
        }

        CClassObjectType ICClassObject.Type()
        {
            return CClassObjectType.AppStabilizer;
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
