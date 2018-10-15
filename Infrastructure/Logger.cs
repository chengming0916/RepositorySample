using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;

namespace Infrastructure
{
    public class Logger
    {
        private ILog logger;
        static Logger()
        {
            Default = new Logger(LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType));
        }

        private Logger(string name) : this(LogManager.GetLogger(name)) { }

        public Logger(ILog logger)
        {
            this.logger = logger;
        }

        public static Logger Default { get; private set; }

        public static Logger GetLogger(Type type)
        {
            return new Logger(LogManager.GetLogger(type));
        }

        public void Debug(string msg, Exception ex)
        {
            logger.Debug(msg, ex);
        }

        public void Info(string msg, Exception ex = null)
        {
            logger.Info(msg, ex);
        }

        public void Warn(string msg, Exception ex = null)
        {
            logger.Warn(msg, ex);
        }

        public void Error(string msg, Exception ex)
        {
            logger.Error(msg, ex);
        }

        public void Fatal(string msg, Exception ex)
        {
            logger.Fatal(msg, ex);
        }
    }
}
