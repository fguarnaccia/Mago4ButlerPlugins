using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.Log
{
    public static class LoggerTrait
    {
        static Dictionary<ILogger, LoggerService> loggers = new Dictionary<ILogger, LoggerService>();

        public static void LogInfo(this ILogger @this, string message)
        {
            LoggerService loggerService = null;
            if (!loggers.TryGetValue(@this, out loggerService))
            {
                loggerService = new LoggerService();
                loggers.Add(@this, loggerService);
            }
            loggerService.LogInfo(message);
        }
        public static void LogError(this ILogger @this, string message, Exception exc = null)
        {
            LoggerService loggerService = null;
            if (!loggers.TryGetValue(@this, out loggerService))
            {
                loggerService = new LoggerService();
                loggers.Add(@this, loggerService);
            }
            loggerService.LogError(message, exc);
        }
    }
}
