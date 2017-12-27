using log4net;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections;
namespace Logging
{
    public class Log4NetLogger : ILogger
    {
        #region ILogger Members

        private static log4net.ILog log = null;
        private string loggerName = "root";
        private string serverName = "";
        private int defaultLevel = LogGlobals.LOG_INFO;
        public int overriderLevel = -1;
        private CustomFileAppender customAppender = null;
        private string overrideConversionPattern = "";
        private string fullMsgConversionPattern = "%date [%thread] %-5level %logger - %message%newline";
        private string defaultConversionPattern = "%date [%thread] %-5level %logger - %.2500message%newline";
        // public static Log4NetLogger sharedInstance;

        /*   public static Log4NetLogger GetInstance()
           {
               if (sharedInstance == null)
               {
                   sharedInstance = new Log4NetLogger();
               }
               return sharedInstance;
           }*/

        public Log4NetLogger()
        {
            log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); 
            log4net.Config.XmlConfigurator.Configure();
            SetDefaultLogLevel();
            SetDefaultConversionPattern();
        }

        public Log4NetLogger(string className)
        {
            log = log4net.LogManager.GetLogger(className);
            log4net.Config.XmlConfigurator.Configure();
            loggerName = className;
            SetDefaultLogLevel();
            SetDefaultConversionPattern();
        }

        public Log4NetLogger(string className,string serverName,int? logLevel)
        {
            string name = "";
            if (serverName != null)
            {
                name = serverName + "_" + className + "Appender";
                loggerName = serverName + "_" + className;
            }
            else
            {
                loggerName = className;
                name = className + "Appender";
            }
            log4net.Appender.RollingFileAppender appender = Logger.GetAppenders(className, serverName);
            if (appender == null) {
                customAppender = new CustomFileAppender(className, serverName, logLevel);
            }
            log = log4net.LogManager.GetLogger(loggerName);
     //       log4net.ILog[] loggers = log4net.LogManager.GetCurrentLoggers();
          //  log4net.Config.XmlConfigurator.Configure();
            this.serverName = serverName;
            SetDefaultLogLevel();
            SetDefaultConversionPattern();
            if(logLevel != null)
              SetLogLevel((int)logLevel);
        }


        public void Information(string message, Exception ex = null)
        {
            if (serverName.Length > 0)
                message = serverName + ":" + loggerName + ":" + message;
            else
                message = loggerName + ":" + message;

            if (ex == null)
            {
                log.Info(message);
            }
            else
            {
                log.Info(message + "\n" + (ex.Message ?? "")
                    + (ex.InnerException == null ? "\n" : "\nInner exception:" + ex.InnerException.Message ?? "" + "\n")
                    + ex.StackTrace ?? "");
            }
        }

        public void Warning(string message, Exception ex = null)
        {
            if (serverName.Length > 0)
                message = serverName + ":" + loggerName + ":" + message;
            else
                message = loggerName + ":" + message;

            if (ex == null)
            {
                log.Warn(message);
            }
            else
            {
                log.Warn(message + "\n" + (ex.Message ?? "")
                    + (ex.InnerException == null ? "\n" : "\nInner exception:" + ex.InnerException.Message ?? "" + "\n")
                    + ex.StackTrace ?? "");
            }
        }

        public void Error(string message, Exception ex = null)
        {
            if (serverName.Length > 0)
                message = serverName + ":" + loggerName + ":" + message;
            else
                message = loggerName + ":" + message;

            if (ex == null)
            {
                log.Error(message);
            }
            else
            {
                log.Error(message + "\n" + (ex.Message ?? "")
                    + (ex.InnerException == null ? "\n" : "\nInner exception:" + ex.InnerException.Message ?? "" + "\n")
                    + ex.StackTrace ?? "");
            }
        }

        public void Fatal(string message, Exception ex = null)
        {
            if (serverName.Length > 0)
                message = serverName + ":" + loggerName + ":" + message;
            else
                message = loggerName + ":" + message;

            if (ex == null)
            {
                log.Fatal(message);
            }
            else
            {
                log.Fatal(message + "\n" + (ex.Message ?? "")
                    + (ex.InnerException == null ? "\n" : "\nInner exception:" + ex.InnerException.Message ?? "" + "\n")
                    + ex.StackTrace ?? "");
            }
        }
        public void All(string message, Exception ex = null)
        {
            if (serverName.Length > 0)
                message = serverName + ":" + loggerName + ":" + message;
            else
                message = loggerName + ":" + message;

            if (ex == null)
            {
               log.Debug(message);
            }
            else
            {
                log.Debug(message + "\n" + (ex.Message ?? "")
                    + (ex.InnerException == null ? "\n" : "\nInner exception:" + ex.InnerException.Message ?? "" + "\n")
                    + ex.StackTrace ?? "");
            }
        }

        public void Debug(string message, Exception ex = null)
        {
            if (serverName.Length > 0)
                message = serverName + ":" + loggerName + ":" + message;
            else
                message = loggerName + ":" + message;

            if (ex == null)
            {
                 log.Debug(message);
            }
            else
            {
                log.Debug(message + "\n" + (ex.Message ?? "")
                    + (ex.InnerException == null ? "\n" : "\nInner exception:" + ex.InnerException.Message ?? "" + "\n")
                    + ex.StackTrace ?? "");
            }
        }

        public void Trace(string message, Exception ex = null)
        {
            if (serverName.Length > 0)
                message = serverName + ":" + loggerName + ":" + message;
            else
                message = loggerName + ":" + message;

            if (ex == null)
            {
                log.Debug(message);
            }
            else
            {
                log.Debug(message + "\n" + (ex.Message ?? "")
                    + (ex.InnerException == null ? "\n" : "\nInner exception:" + ex.InnerException.Message ?? "" + "\n")
                    + ex.StackTrace ?? "");
            }
        }

        public void ResetConversionPattern(bool useOverride = false)
        {
            var appenders = ((log4net.Repository.Hierarchy.Logger)log.Logger).Appenders;
            foreach (var rollingFileAppender in appenders.OfType<log4net.Appender.RollingFileAppender>())
            {
                if (((log4net.Repository.Hierarchy.Logger)log.Logger).Level == log4net.Core.Level.Debug ||
                   ((log4net.Repository.Hierarchy.Logger)log.Logger).Level == log4net.Core.Level.All)
                    rollingFileAppender.Layout = new log4net.Layout.PatternLayout(fullMsgConversionPattern);
                else if (overrideConversionPattern.Length > 0 && useOverride)
                    rollingFileAppender.Layout = new log4net.Layout.PatternLayout(overrideConversionPattern);
                else {
                    rollingFileAppender.Layout = new log4net.Layout.PatternLayout(defaultConversionPattern);
                    overrideConversionPattern = "";
                }
            break;
            }
        }

        private void SetFullMsgPattern()
        {
            var appenders = ((log4net.Repository.Hierarchy.Logger)log.Logger).Appenders;
            foreach (var rollingFileAppender in appenders.OfType<log4net.Appender.RollingFileAppender>())
            {
                rollingFileAppender.Layout = new log4net.Layout.PatternLayout(fullMsgConversionPattern);
                break;
            }
        }


        public void LogFullMsg()
        {
            int msgLen = -1;
            SetLogMsgLength(msgLen);
        }


        public void SetLogMsgLength(int length)
        {

            var appenders = ((log4net.Repository.Hierarchy.Logger)log.Logger).Appenders;
            foreach (var rollingFileAppender in appenders.OfType<log4net.Appender.RollingFileAppender>())
            {
                log4net.Layout.PatternLayout layout = (log4net.Layout.PatternLayout)rollingFileAppender.Layout;

                string conversionPattern = layout.ConversionPattern;
                int msgLocation = conversionPattern.IndexOf("message");
                if (msgLocation != -1)
                {
                    if (msgLocation > 0)
                    {
                        int pctLocation = conversionPattern.Substring(0, msgLocation).LastIndexOf("%");
                        if (pctLocation != -1)
                        {
                            string msgParms = conversionPattern.Substring(pctLocation, msgLocation - pctLocation);
                            int dotLocation = msgParms.IndexOf(".");
                            if(dotLocation > -1)
                            {
                                string dotVal = msgParms.Substring(0, dotLocation);
                                if (length > 0)
                                {
                                    dotVal += "." + length.ToString();
                             //       dotVal = dotVal + conversionPattern.Substring(msgLocation);
                            //        conversionPattern = conversionPattern.Substring(0, pctLocation) + dotVal;
                                }
                               /* else
                                {
                                    conversionPattern = conversionPattern.Substring(0, pctLocation) + dotVal;
                                }*/
                                dotVal = dotVal + conversionPattern.Substring(msgLocation);
                                conversionPattern = conversionPattern.Substring(0, pctLocation) + dotVal;
                            }
                            else
                            {
                                if (length > 0)
                                {
                                    conversionPattern = conversionPattern.Substring(0, pctLocation) + "." + length + conversionPattern.Substring(msgLocation);
                                }
                                else
                                {
                                    conversionPattern = conversionPattern.Substring(0, pctLocation) + "." + conversionPattern.Substring(msgLocation);
                                }
                               
                                    
                            }
                        }
                    }
                    rollingFileAppender.Layout = new log4net.Layout.PatternLayout(conversionPattern);
                    overrideConversionPattern = conversionPattern;
                }
            }
        }

        public void SetLogLevel(int level)
        {
            bool resetPattern = false;
            if (((log4net.Repository.Hierarchy.Logger)log.Logger).Level == log4net.Core.Level.Debug ||
                   ((log4net.Repository.Hierarchy.Logger)log.Logger).Level == log4net.Core.Level.All)
                resetPattern = true;

                if (level == LogGlobals.LOG_DEFAULT)
            {
                SetLogLevel(defaultLevel);
                overriderLevel = -1;
                ResetConversionPattern();
            }
            else if (level == LogGlobals.LOG_ALL)
            {
                ((log4net.Repository.Hierarchy.Logger)log.Logger).Level = log4net.Core.Level.All;
                overriderLevel = level;
                SetFullMsgPattern();
            }
            else if (level == LogGlobals.LOG_DEBUG)
            {
                ((log4net.Repository.Hierarchy.Logger)log.Logger).Level = log4net.Core.Level.Debug;
                overriderLevel = level;
                SetFullMsgPattern();
            }
            else if (level == LogGlobals.LOG_INFO)
            {
                ((log4net.Repository.Hierarchy.Logger)log.Logger).Level = log4net.Core.Level.Info;
                overriderLevel = level;
                if(resetPattern)
                  ResetConversionPattern(true);
            }
            else if (level == LogGlobals.LOG_WARNING)
            {
                ((log4net.Repository.Hierarchy.Logger)log.Logger).Level = log4net.Core.Level.Warn;
                overriderLevel = level;
                if (resetPattern)
                    ResetConversionPattern(true);
            }
            else if (level == LogGlobals.LOG_ERROR)
            {
                ((log4net.Repository.Hierarchy.Logger)log.Logger).Level = log4net.Core.Level.Error;
                overriderLevel = level;
                if (resetPattern)
                    ResetConversionPattern(true);
            }
            else if (level == LogGlobals.LOG_FATAL)
            {
                ((log4net.Repository.Hierarchy.Logger)log.Logger).Level = log4net.Core.Level.Fatal;
                overriderLevel = level;
                if (resetPattern)
                    ResetConversionPattern(true);
            }
        }

        private void SetDefaultLogLevel()
        {
            if (log.IsDebugEnabled)
                defaultLevel = LogGlobals.LOG_DEBUG;
            else if (log.IsInfoEnabled)
                defaultLevel = LogGlobals.LOG_INFO;
            else if (log.IsWarnEnabled)
                defaultLevel = LogGlobals.LOG_WARNING;
            else if (log.IsErrorEnabled)
                defaultLevel = LogGlobals.LOG_ERROR;
            else if (log.IsFatalEnabled)
                defaultLevel = LogGlobals.LOG_FATAL;
        }

        private void SetDefaultConversionPattern()
        {
            var appenders = ((log4net.Repository.Hierarchy.Logger)log.Logger).Appenders;
            foreach (var rollingFileAppender in appenders.OfType<log4net.Appender.RollingFileAppender>())
            {
                log4net.Layout.PatternLayout layout = (log4net.Layout.PatternLayout)rollingFileAppender.Layout;

                defaultConversionPattern = layout.ConversionPattern;
                break;
            }
        }

        private string parseUntruncatedPattern(string conversionPattern)
        {
            if (conversionPattern == null)
                return "";

            int msgLocation = conversionPattern.IndexOf("message");
            if (msgLocation != -1)
            {
                if (msgLocation > 0)
                {
                    int pctLocation = conversionPattern.Substring(0, msgLocation).LastIndexOf("%");
                    if (pctLocation != -1)
                    {
                        string msgParms = conversionPattern.Substring(pctLocation, msgLocation - pctLocation);
                        int dotLocation = msgParms.IndexOf(".");
                        if (dotLocation > -1)
                        {
                            string dotVal = msgParms.Substring(0, dotLocation);
                            dotVal = dotVal + conversionPattern.Substring(msgLocation);
                            conversionPattern = conversionPattern.Substring(0, pctLocation) + dotVal;
                        }
                    }
                }
            }
            return conversionPattern;
        }

        public void SetLogger(ILogger logger)
        {

        }

        public void Reset(string className)
        {
            log = log4net.LogManager.GetLogger(className);
            log4net.Config.XmlConfigurator.Configure();
            loggerName = className;
            SetDefaultLogLevel();
            SetDefaultConversionPattern();
            ResetConversionPattern(true);
            if (overriderLevel != defaultLevel)
                SetLogLevel(overriderLevel);
        }

        public void Reset(string className,string serverName)
        {
            loggerName = serverName + "_" + className;
            log4net.Appender.RollingFileAppender appender = Logger.GetAppenders(className, serverName);
            if (appender == null)
            {
                customAppender = new CustomFileAppender(className, serverName, overriderLevel);
            }
            log = log4net.LogManager.GetLogger(loggerName);
            SetDefaultLogLevel();
            SetDefaultConversionPattern();
            if (overriderLevel != defaultLevel)
                SetLogLevel(overriderLevel);
        }
    }
        #endregion
}
