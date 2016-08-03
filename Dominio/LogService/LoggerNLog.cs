using NLog;

namespace Dominio.LogService
{
    public static class LoggerNLog
    {
            public static Logger logger = LogManager.GetCurrentClassLogger();
    }
}
