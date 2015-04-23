using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class LogHelper
    {
        private static ILog logger = null;

        public LogHelper(Type t)
        {
            logger = LogManager.GetLogger(t);
        }

        public LogHelper(string name)
        {
            logger = LogManager.GetLogger(name);
        }

        public void Debug(string msg)
        {
            logger.Debug(msg);
        }

        public void Error(string msg)
        {
            logger.Error(msg);
        }

        public void Error(Exception ex)
        {
            logger.Error(ex.Message + ex.StackTrace);
        }

        public void Warn(string msg)
        {
            logger.Warn(msg);
        }
    }
}
