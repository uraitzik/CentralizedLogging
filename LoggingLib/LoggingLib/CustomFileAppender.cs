using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging
{
    class CustomFileAppender
    {
        private string appenderName = "";
        private string className = "";
        private string serverName = "";
        private string logFileName = "";
        private int logLevel = LogGlobals.LOG_INFO;
        private int defaultLevel = LogGlobals.LOG_INFO;
        private string fullMsgConversionPattern = "%date [%thread] %-5level %logger - %message%newline";
        public CustomFileAppender(string i_ClassName, string i_ServerName, int? i_LogLevel, string pattern = null)
        {
            if (i_ClassName == null | i_ClassName.Length < 1)
                i_ClassName = "*";
            className = i_ClassName;
            if (i_LogLevel != null)
                logLevel = (int)i_LogLevel;
            if (i_ServerName != null)
                serverName = i_ServerName;
            logFileName = "LOG_" + serverName + "_" + className + "_.log";
            appenderName = serverName + "_" + className + "Appender";
            addNewAppender(logLevel, pattern);
        }

        private void addNewAppender(int? logLevel, string pattern)
        {
            RollingFileAppender classAppender = Logger.GetAppenders(className, null);
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
            PatternLayout patternLayout = new PatternLayout();
            if (pattern == null) {
                if(classAppender != null)
                {
                    if(classAppender.Layout != null)
                    {
                        log4net.Layout.PatternLayout layout = (log4net.Layout.PatternLayout)classAppender.Layout;
                        patternLayout.ConversionPattern = layout.ConversionPattern;
                    }
                    else
                        patternLayout.ConversionPattern = fullMsgConversionPattern;
                }
                else
                   patternLayout.ConversionPattern = fullMsgConversionPattern;
            }
            else
                patternLayout.ConversionPattern = pattern;
            patternLayout.ActivateOptions();

            RollingFileAppender roller = new RollingFileAppender();
            roller.Layout = patternLayout;
            roller.Name = appenderName;
            roller.RollingStyle = RollingFileAppender.RollingMode.Composite;
            roller.PreserveLogFileNameExtension = true;
            roller.StaticLogFileName = false;
            roller.AppendToFile = true;
            roller.File = logFileName;
            roller.DatePattern = "yyyyMMdd-HH";
            if (classAppender != null)
            {
             //   roller.File = classAppender.File;
                roller.MaxSizeRollBackups = classAppender.MaxSizeRollBackups;
                roller.MaximumFileSize = classAppender.MaximumFileSize;
                roller.Threshold = classAppender.Threshold;
            }
            else
            {
               roller.MaxSizeRollBackups = 100;
                roller.MaximumFileSize = "10MB";
            }
            roller.ActivateOptions();


            log4net.ILog log = log4net.LogManager.GetLogger(serverName + "_" + className);
            ((log4net.Repository.Hierarchy.Logger)log.Logger).AddAppender(roller);
           // hierarchy.Root.AddAppender(roller);

          //  hierarchy.Root.Level = GetLogLevel(logLevel);
            hierarchy.Configured = true;
        }

        private log4net.Core.Level GetLogLevel(int level)
        {
            if (level == LogGlobals.LOG_DEFAULT)
            {
                return GetLogLevel(defaultLevel);
            }
            else if (level == LogGlobals.LOG_ALL)
            {
                return log4net.Core.Level.All;
            }
            else if (level == LogGlobals.LOG_DEBUG)
            {
                return log4net.Core.Level.Debug;
            }
            else if (level == LogGlobals.LOG_INFO)
            {
                return log4net.Core.Level.Info;
            }
            else if (level == LogGlobals.LOG_WARNING)
            {
                return log4net.Core.Level.Warn;
            }
            else if (level == LogGlobals.LOG_ERROR)
            {
                return log4net.Core.Level.Error;
            }
            else if (level == LogGlobals.LOG_FATAL)
            {
                return log4net.Core.Level.Fatal;
            }
            return GetLogLevel(defaultLevel);
        }
    }
}
