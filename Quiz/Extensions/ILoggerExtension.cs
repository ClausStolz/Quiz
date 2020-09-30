using System;

using Microsoft.Extensions.Logging;


namespace Quiz.Extensions
{
    public static class ILoggerExtension
    {
        public static void WriteLog(this ILogger logger, string message)
        {
            logger.LogInformation(message);
        }

        public static void WriteLog(this ILogger logger, Exception ex, string message)
        {
            logger.LogError(ex, message);
        }
    }
}