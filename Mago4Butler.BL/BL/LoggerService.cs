using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.BL
{
    public class LoggerService
    {
        ILog log;

        public LoggerService()
        {
            this.log = LogManager.GetLogger("Mago4ButlerLogger");
        }

        public void LogError(string message, Exception exc = null)
        {
            if (this.log != null)
            {
                this.log.Error(message, exc);
            }
        }

        public void LogInfo(string message)
        {
            if (this.log != null)
            {
                this.log.Info(message);
            }
        }
    }
}
