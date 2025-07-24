using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ConfigTool
{
    public static class Log
    {
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public static void Debug(string msg)
        {
            _logger.Debug(msg);
        }

        public static void Error(string msg)
        {
            _logger.Error(msg);
        }

    }
}
