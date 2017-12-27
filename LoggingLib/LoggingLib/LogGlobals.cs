using System;

namespace Logging
{
    public static class LogGlobals
    {
        public const string MAX_FILE_SIZE = "MAX_FILE_SIZE";
        public const string LOG_FILE_NAME = "LOG_FILE_NAME";
        public const string LOG_FILE_PATH= "LOG_FILE_PATH";
        public const string LOG_LEVEL = "LOG_LEVEL";
        public const string MAX_MSG_LENGTH = "MAX_MSG_LENGTH";
        //log levels
        public const int LOG_FATAL = 1;
        public const int LOG_ERROR = 2;
        public const int LOG_WARNING = 3;
        public const int LOG_INFO = 4;
        public const int LOG_TRACE = 5;
        public const int LOG_DEBUG = 6;
        public const int LOG_ALL = 7;
        public const int LOG_DEFAULT = 8;

        static LogGlobals()
        {
        }

        public static string GetServerName()
        {
            try
            {
                return System.Net.Dns.GetHostName();
            }catch(Exception exc)
            {
                return System.Environment.MachineName;
            }
        }
    }
}