using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMusicStore.Logging
{
    public class Logger: IUserLogger
    {
        private static NLog.ILogger logger;
        public Logger()
        {
            logger = LogManager.GetLogger("Logger");
        }       

        public void Error(string message)
        {
            logger.Error(message);
        }

        public void Info(string message)
        {
            logger.Info(message);
        }

        public void Debug(string message)
        {
            logger.Debug(message);
        }

        public void Trace(string message)
        {
            logger.Trace(message);
        }

        public void Warn(string message)
        {
            logger.Warn(message);
        }

        public void Fatal(string message)
        {
            logger.Fatal(message);
        }
    }
}