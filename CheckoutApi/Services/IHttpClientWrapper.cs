using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CheckoutApi.Models;

namespace CheckoutApi.Services
{
    public interface IHttpClientWrapper
    {
        Task<HttpWrappedResponse<T>> GetAsync<T>(string requestUri, HttpMethod method, CancellationToken cancellationToken = default, object requestBody = null, Dictionary<string, string> customRequestHeaders = null, ContentType contentType = ContentType.Json);
    }
}