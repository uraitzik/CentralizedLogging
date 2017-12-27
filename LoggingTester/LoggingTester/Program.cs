using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingTester
{
    class Program
    {
        static void Main(string[] args)
        {
            string line;
            while((line = Console.ReadLine()) != "Exit")
            {
                try
                {
                    string[] tokens = line.Split(' ');
                    if (tokens[0].ToUpper().Equals("LOG"))
                    {
                        string level = tokens[1];
                        string className = tokens[2];
                        string logMsg = tokens[3];
                        if (tokens.Length > 3)
                        {
                            for (int i = 4; i < tokens.Length; i++)
                                logMsg = logMsg + " " + tokens[i];
                        }

                        if (level.ToUpper().Equals("INFO"))
                            Logging.Logger.Information(className, logMsg);
                        else if (level.ToUpper().Equals("WARNING") || level.ToUpper().Equals("WARN"))
                            Logging.Logger.Warning(className, logMsg);
                        else if (level.ToUpper().Equals("FATAL"))
                            Logging.Logger.Fatal(className, logMsg);
                        else if (level.ToUpper().Equals("DEBUG"))
                            Logging.Logger.Debug(className, logMsg);
                        else if (level.ToUpper().Equals("ERROR"))
                            Logging.Logger.Error(className, logMsg);
                    }
                    else if (tokens[0].ToUpper().Equals("ADD"))
                    {
                        string className = tokens[1];
                        string serverName = tokens[2];
                        string level = tokens[3];
                        if (level.ToUpper().Equals("INFO"))
                            Logging.Logger.AddLogger(className, serverName, Logging.LogGlobals.LOG_INFO);
                        else if (level.ToUpper().Equals("WARNING") || level.ToUpper().Equals("WARN"))
                            Logging.Logger.AddLogger(className, serverName, Logging.LogGlobals.LOG_WARNING);
                        else if (level.ToUpper().Equals("FATAL"))
                            Logging.Logger.AddLogger(className, serverName, Logging.LogGlobals.LOG_FATAL);
                        else if (level.ToUpper().Equals("DEBUG"))
                            Logging.Logger.AddLogger(className, serverName, Logging.LogGlobals.LOG_DEBUG);
                        else if (level.ToUpper().Equals("ERROR"))
                            Logging.Logger.AddLogger(className, serverName, Logging.LogGlobals.LOG_ERROR);

                    }
                    else if (tokens[0].ToUpper().Equals("REMOVE"))
                    {
                        string className = tokens[1];
                        string serverName = tokens[2];
                        Logging.Logger.RemoveLogger(className, serverName);
                    }
                    else if (tokens[0].ToUpper().Equals("SET"))
                    {
                        if (tokens[1].ToUpper().Equals("LENGTH"))
                        {
                            string className = tokens[2];
                            string length = tokens[3];
                            Logging.Logger.SetLogMsgLength(className, int.Parse(length));

                        }
                        else if (tokens[1].ToUpper().Equals("LEVEL"))
                        {
                            string className = tokens[2];
                            string level = tokens[3];
                            if (level.ToUpper().Equals("INFO"))
                                Logging.Logger.SetLogLevel(className, Logging.LogGlobals.LOG_INFO);
                            else if (level.ToUpper().Equals("WARNING") || level.ToUpper().Equals("WARN"))
                                Logging.Logger.SetLogLevel(className, Logging.LogGlobals.LOG_WARNING);
                            else if (level.ToUpper().Equals("FATAL"))
                                Logging.Logger.SetLogLevel(className, Logging.LogGlobals.LOG_FATAL);
                            else if (level.ToUpper().Equals("DEBUG"))
                                Logging.Logger.SetLogLevel(className, Logging.LogGlobals.LOG_DEBUG);
                            else if (level.ToUpper().Equals("ERROR"))
                                Logging.Logger.SetLogLevel(className, Logging.LogGlobals.LOG_ERROR);
                            else if (level.ToUpper().Equals("DEFAULT"))
                                Logging.Logger.SetLogLevel(className, Logging.LogGlobals.LOG_DEFAULT);
                            else
                                Logging.Logger.SetLogLevel(className, int.Parse(level));

                        }
                    }
                    else if (tokens[0].ToUpper().Equals("RESET"))
                    {
                        string className = tokens[1];
                        Logging.Logger.ResetConversionPattern(className, false);
                    }
                }catch(Exception exc)
                {

                }
            }
        }
    }
}

