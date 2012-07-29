using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using log4net.Config;
using Jumbleblocks.Core.Logging;

namespace Jumbleblocks.Website.TempLogger
{
    public class Logger : IJumbleblocksLogger
    {
        private static readonly ILog _logger = LogManager.GetLogger("Temp_Logger");

        static Logger()
        {
            BasicConfigurator.Configure();
        }

        public void LogInfo(string message, Exception ex = null)
        {
            _logger.Info(message, ex);
        }

        public void LogDebug(string message, Exception ex = null)
        {
            _logger.Debug(message, ex);
        }

        public void LogWarn(string message, Exception ex = null)
        {
            _logger.Warn(message, ex);
        }

        public void LogError(string message, Exception ex = null)
        {
            _logger.Error(message, ex);
        }

        public void LogFatal(string message, Exception ex = null)
        {
            _logger.Fatal(message, ex);
        }
    }
}