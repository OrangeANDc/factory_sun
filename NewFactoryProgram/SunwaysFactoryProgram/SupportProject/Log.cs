namespace SupportProject
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
