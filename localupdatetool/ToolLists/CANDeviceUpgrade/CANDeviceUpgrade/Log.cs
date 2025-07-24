using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NLog;

namespace CANDeviceUpgrade
{
    public static class Log
    {
        private static NLog.ILogger _logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Debug(string msg)
        {
            try
            {
                _logger.Debug(msg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

    public static class Method
    {
        public static string ByteArrayToSting(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return "empty";
            string result = string.Empty;
            foreach (var b in bytes)
            {
                result += (b.ToString("x2") + " ");
            }

            return result;
        }
    }
}
