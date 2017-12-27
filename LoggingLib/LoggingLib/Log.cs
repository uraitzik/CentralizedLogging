using log4net;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging
{
    public  class Log
    {
        private  Dictionary<string, ILogger> loggerDictionary;
        private  Dictionary<string, Dictionary<string, ILogger>> serverLoggerDictionary;
        public static Log sharedInstance;

        public static Log GetInstance()
        {
            if (sharedInstance == null)
            {
                sharedInstance = new Log();
            }
            return sharedInstance;
        }


        public Log()
        {
            loggerDictionary = new Dictionary<string, ILogger>();
            serverLoggerDictionary = new Dictionary<string, Dictionary<string, ILogger>>();
        }

        public  ILogger Logger()
        {
            if (loggerDictionary.ContainsKey("root"))
                return loggerDictionary["root"];

            return createLogger("root");
        }

        private ILogger createLogger(string className)
        {
            if(className == null || className.Length < 1 || className.Equals("root"))
            {
                ILogger netLogger = new Log4NetLogger();
                loggerDictionary.Add("root", netLogger);
                return netLogger;
            }
            ILogger newLogger = new Log4NetLogger(className);
            loggerDictionary.Add(className, newLogger);
            return newLogger;
        }

        private ILogger createLogger(string className, string serverName, int? logLevel)
        {
            if (className == null || className.Length < 1)
                className = "*";

            Log4NetLogger logger = new Log4NetLogger(className, serverName,logLevel);
            return logger;
        }
       

        public ILogger Logger(string className)
        {
            string serverName = LogGlobals.GetServerName();
            if(serverLoggerDictionary.ContainsKey(serverName))
            {
                ILogger theLog = serverLoggerDictionary[serverName][className];
                if (theLog != null)
                {
                    theLog.Reset(className, serverName);
                    return theLog;
                }
            }
            if (loggerDictionary.ContainsKey(className))
            {
                loggerDictionary[className].Reset(className);
                return loggerDictionary[className];
            }

            return createLogger(className);
        }

        //get the logger for the specific values, and  set its log level to the log level included.
        public ILogger Logger(string className, string serverName, int? logLevel = null)
        {
            Log4NetLogger serverLogger = null;
            Dictionary<string, ILogger> dictionary = null;
            if (!LogGlobals.GetServerName().Equals(serverName))
                return Logger(className);
            if (serverLoggerDictionary.ContainsKey(serverName))
            {
                dictionary = serverLoggerDictionary[serverName];
                if (dictionary.ContainsKey(className))
                {

                    serverLogger = (Log4NetLogger)dictionary[className];
                    if (logLevel != null)
                        serverLogger.SetLogLevel((int)logLevel);
                    dictionary.Add(className, serverLogger);
                    serverLoggerDictionary.Add(serverName, dictionary);
                    serverLogger.Reset(className, serverName);
                    return serverLogger;

                }
                else
                {
                    serverLogger = (Log4NetLogger)createLogger(className, serverName, logLevel);
                    dictionary.Add(className, serverLogger);
                    serverLoggerDictionary.Add(serverName, dictionary);
                    return serverLogger;
                }
            }
            else
            {
                serverLogger = (Log4NetLogger)createLogger(className, serverName, logLevel);
                dictionary = new Dictionary<string, ILogger>();
                dictionary.Add(className, serverLogger);
                serverLoggerDictionary.Add(serverName, dictionary);
                return serverLogger;
            }
        }

        //Remove override logging for the specific server and product combination
        public void RemoveLogger(string className, string serverName)
        {
            Dictionary<string, ILogger> dictionary = serverLoggerDictionary[serverName];
            if(dictionary != null)
            {
                if(dictionary.ContainsKey(className))
                {
                    dictionary.Remove(className);
                    if(dictionary.Count < 1)
                    {
                        serverLoggerDictionary.Remove(serverName);
                    }
                    else
                    {
                        serverLoggerDictionary.Add(serverName, dictionary);
                    }
                }
                Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
                string appenderName = className + "_" + serverName + "Appender";
                hierarchy.Root.RemoveAppender(appenderName);
                if (loggerDictionary.ContainsKey(className))
                    loggerDictionary[className].Reset(className) ;

            }
        }

        public log4net.Appender.RollingFileAppender GetAppenders(string className, string serverName = null)
        {
            log4net.Appender.IAppender[] appenders = LogManager.GetRepository().GetAppenders();
            string name = "";
            if (serverName != null)
                name = serverName  + "_" + className + "Appender";
             else
              name = className + "Appender";
            foreach(log4net.Appender.IAppender appender in appenders )
            {
                if (appender.Name.Equals(name))
                    return (log4net.Appender.RollingFileAppender)appender;
            }
            return null;
        }

        public void GetCurrentLoggers()
        {
            log4net.ILog[] loggers = log4net.LogManager.GetCurrentLoggers();
            return;
        }

        public void SetLogger(string className, ILogger logger)
        {
            loggerDictionary.Add(className, logger);
        }
    }
}
