﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Serilog.Core;

namespace Kit
{
    public class Log
    {
        public static ILogger Logger => Current._Logger;
        private ILogger _Logger;


        public string CriticalLoggerPath
        {
            get;
            private set;
        }
        public string LoggerPath
        {
            get;
            private set;
        }
        public bool AlertAfterCritical
        {
            get;
            private set;
        }
        private EventHandler OnAlertCritical;
        public event EventHandler OnConecctionLost;


        public static Log Current
        {
            get;
            private set;
        }

        private Log() { }

        public static Log Init(DirectoryInfo LogDirectory = null)
        {
            string log_path = Path.Combine(LogDirectory?.FullName ?? Kit.Tools.Instance.LibraryPath, "Logs");
            DirectoryInfo logDirectory = new DirectoryInfo(log_path);
            if (!logDirectory.Exists)
            {
                logDirectory.Create();
            }
            Current = new Log()
            {
                LoggerPath = Path.Combine(logDirectory.FullName, "log.log"),
                CriticalLoggerPath = Path.Combine(logDirectory.FullName, "critcal_log.log")
            };
            return Current;
        }


        public void SetLogger(ILogger Logger, EventHandler CriticalAction = null)
        {
            Current._Logger = Logger;
            Current.OnAlertCritical = CriticalAction;

            if (Current.OnAlertCritical != null)
            {
                AlertCriticalUnhandled();
            }
        }

        #region Errores
        public static void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
        {
            Exception newExc = new Exception("TaskSchedulerOnUnobservedTaskException", unobservedTaskExceptionEventArgs.Exception);
            LogUnhandledException(newExc);
        }

        public static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            Exception newExc = new Exception("CurrentDomainOnUnhandledException", unhandledExceptionEventArgs.ExceptionObject as Exception);
            LogUnhandledException(newExc);
        }

        internal static void LogUnhandledException(Exception exception)
        {
            try
            {
                string errorMessage = $"-->[{DateTime.Now}]Error: ►►{exception.ToString()}◄◄";
                Console.WriteLine(errorMessage);
                Logger.Fatal(exception, errorMessage);
            }
            catch
            {
                // just suppress any error logging exceptions
            }
        }
        #endregion
        private static void AlertCriticalUnhandled()
        {
            FileInfo file = new FileInfo(Current.CriticalLoggerPath);
            if (file.Exists)
            {
                using (var fs = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var sr = new StreamReader(fs, Encoding.Default))
                {
                    string criticalDescription = sr.ReadToEnd();
                    if (!string.IsNullOrEmpty(criticalDescription))
                    {
                        Log.Logger.Error(criticalDescription);
                        Current.OnAlertCritical?.Invoke(criticalDescription, EventArgs.Empty);
                        file.Delete();
                    }
                }
            }
        }
        public static bool AlertOnDBConnectionError(Exception ex)
        {
            if (IsDBConnectionError(ex))
            {
                Current.OnConecctionLost?.Invoke(ex, EventArgs.Empty);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Retorna la excepcion base de donde se origino el error
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static Exception MainExcepcion(Exception ex)
        {
            Exception Exbase = ex;
            while (Exbase?.InnerException != null)
            {
                Exbase = Exbase.InnerException;
            }
            return Exbase;
        }
        public static bool IsDBConnectionError(Exception ex)
        {
            Exception Exbase = MainExcepcion(ex);
            Exception exception = ex;
#if NETSTANDARD
            bool desconexion = (exception is SqlException);
#else
            bool desconexion = (exception.GetType().Name == "SqlException");
#endif
            if (!desconexion)
            {
                //Asegurarse
                foreach (string identificados in new string[] { "INVALID OBJECT NAME", "FK_DESCARGAS", "INVALID COLUMN NAME", "SOCKET", "TIMEOUT" })
                    if (
                        (exception?.Message?.ToUpper()?.Contains(identificados) ?? false)
                        ||
                        (ex?.Message?.ToUpper()?.Contains(identificados) ?? false)
                        ||
                        (Exbase?.Message?.ToUpper()?.Contains(identificados) ?? false)
                    )
                    {
                        //Log.LogMe($"-->[WARNING!!!] Se adapto forzadamente por:=>[☺{exception?.Message}☺,☺{ex?.Message}☺,☺{Exbase?.Message}☺]");
                        //AppData.Demonio.AdaptarLaBase();
                        //AppData.Demonio.Despierta();
                        desconexion = true;
                        break;
                    }
            }
            if (desconexion)
            {
                Log.Logger.Warning($"-->[WARNING!!!] DESCONEXION PROVOCADA POR:=>[☺{exception?.Message}☺,☺{ex?.Message}☺,☺{Exbase?.Message}☺]");
            }
            else
            {
                Log.Logger.Warning($"-->[WARNING!!!] FAKE ERROR:=>[☺{exception?.Message}☺,☺{ex?.Message}☺,☺{Exbase?.Message}☺]");
            }
            return desconexion;


        }
    }
}
