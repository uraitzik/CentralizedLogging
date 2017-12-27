using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Repository.Hierarchy;

namespace Logging
{
    public static class Logger
    {
        private static Log loggerImplementation;

        static Logger()
        {
            loggerImplementation = new Log();
        }

        public static void SetLogger(ILogger logger)
        {

            SetLogger("root", logger);
        }
        public static void SetLogger(string className, ILogger logger)
        {

            loggerImplementation.Logger(className).SetLogger(logger);
        }

        public static void Information(string message, Exception ex = null)
        {
            Information("root", message, ex);
        }

        public static void Information(string className, string message, Exception ex = null)
        {
            loggerImplementation.Logger(className).Information(message, ex);
        }

        public static void Warning(string message, Exception ex = null)
        {
            Warning("root", message, ex);
        }

        public static void Warning(string className, string message, Exception ex = null)
        {
            loggerImplementation.Logger(className).Warning(message, ex);
        }

        public static void Error(string message, Exception ex = null)
        {
            Error("root", message, ex);
        }

        public static void Error(string className, string message, Exception ex = null)
        {
            loggerImplementation.Logger(className).Error(message, ex);
        }

        public static void Fatal(string message, Exception ex = null)
        {
            Fatal("root", message, ex);
        }

        public static void Fatal(string className, string message, Exception ex = null)
        {
            loggerImplementation.Logger(className).Fatal(message, ex);
        }

        public static void Debug(string message, Exception ex = null)
        {
            Debug("root", message, ex);
        }

        public static void Debug(string className, string message, Exception ex = null)
        {
            loggerImplementation.Logger(className).Debug(message, ex);
        }

        public static void SetLogLevel(int level)
        {
            SetLogLevel("root", level);
        }

        public static void SetLogLevel(string className, int level)
        {
            loggerImplementation.Logger(className).SetLogLevel(level);
        }

        public static void SetLogMsgLength(string className, int length)
        {
            loggerImplementation.Logger(className).SetLogMsgLength(length);
        }

        public static void All(string message, Exception ex = null)
        {
            All("root", message, ex);
        }

        public static void All(string className, string message, Exception ex = null)
        {
            loggerImplementation.Logger(className).All(message, ex);
        }


        public static void ResetConversionPattern(string className,bool useOvveride = false)
        {
            loggerImplementation.Logger(className).ResetConversionPattern(useOvveride);
        }

        public static void AddLogger(string className, string serverName, int? logLevel)
        {
            loggerImplementation.Logger(className, serverName, logLevel);
        }

        public static void RemoveLogger(string className, string serverName)
        {
            loggerImplementation.RemoveLogger(className, serverName);
        }
        public static string GetServerName()
        {
            return System.Net.Dns.GetHostName();
        }

        public static void GetCurrentLoggers()
        {
             loggerImplementation.GetCurrentLoggers();
        }

        public static log4net.Appender.RollingFileAppender GetAppenders(string className, string serverName)
        {
            return loggerImplementation.GetAppenders(className, serverName);
        }
    }
}
