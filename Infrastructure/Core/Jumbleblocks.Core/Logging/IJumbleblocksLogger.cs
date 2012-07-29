using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jumbleblocks.Core.Logging
{
    public interface IJumbleblocksLogger
    {
        void LogInfo(string message, Exception ex = null);
        void LogDebug(string message, Exception ex = null);
        void LogWarn(string message, Exception ex = null);
        void LogError(string message, Exception ex = null);
        void LogFatal(string message, Exception ex = null);
    }
}
