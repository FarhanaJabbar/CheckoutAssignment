using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CheckoutApi.Services
{
    public class LoggerWrapper<T> : ILoggerWrapper<T> where T : class
    {
        private readonly ILogger<T> _internalLogger;

        public LoggerWrapper(ILogger<T> internalLogger)
        {
            _internalLogger = internalLogger;
        }

        public void LogError(string error)
        {
            _internalLogger.LogError(error);
        }

        public void LogError(string errorMessage, Exception ex)
        {
            _internalLogger.LogError( new EventId(1, "CheckoutAppError"), errorMessage, ex);
        }
    }
}
