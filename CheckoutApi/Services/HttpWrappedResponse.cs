using System.Net;
using System.Net.Http;
using CheckoutApi.Models;

namespace CheckoutApi.Services
{
    public class HttpWrappedResponse<T>
    {
        public HttpWrappedResponse(ApiException error)
        {
            Error = error;
            Success = false;
        }
        public HttpWrappedResponse(T data, HttpResponseMessage responseMessage)
        {
            Data = data;
            RawResponse = responseMessage;
            StatusCode = responseMessage.StatusCode;
            Success = responseMessage.IsSuccessStatusCode;
        }
        public T Data { get; }
        public HttpResponseMessage RawResponse { get; }
        public HttpStatusCode StatusCode { get; }
        public bool Success { get; }
        public ApiException Error { get; }
    }
}
