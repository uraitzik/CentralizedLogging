using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging
{
    public interface ILogger
    {
        void Information(string message, Exception ex = null);

        void Warning(string message, Exception ex = null);

        void Error(string message, Exception ex = null);

        void Fatal(string message, Exception ex = null);

        void Debug(string message, Exception ex = null);

        void All(string message, Exception ex = null);

        void SetLogLevel(int level);
        void SetLogger(ILogger logger);
        void SetLogMsgLength(int length);
        void ResetConversionPattern(bool useOvveride = false);

        void Reset(string className);

        void Reset(string className, string serverName);
    }
}
