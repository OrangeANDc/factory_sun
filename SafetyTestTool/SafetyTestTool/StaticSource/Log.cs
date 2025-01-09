using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafetyTestTool.StaticSource
{
    public static class Log
    {
        private static NLog.ILogger _logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Debug(string msg)
        {
            _logger.Debug(msg);
        }

        public static void Error(string msg)
        {
            _logger.Error(msg);
        }

        public static void Info(string msg)
        {
            _logger.Info(msg);
        }
    }
}
