using System;
using System.Net.Http;

namespace CheckoutApi.Models
{
    public class ApiException : Exception
    {
        public ApiException(string message, Exception innerException = null, HttpResponseMessage responseMessage = null) : base(message, innerException)
        {
            ResponseMessage = responseMessage;
        }

        public HttpResponseMessage ResponseMessage { get; }
    }
}

