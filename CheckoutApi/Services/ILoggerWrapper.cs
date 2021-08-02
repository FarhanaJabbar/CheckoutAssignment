using System;

namespace CheckoutApi.Services
{
    public interface ILoggerWrapper<T>
    {
        void LogError(string error);
        void LogError(string error, Exception ex);
    }
}