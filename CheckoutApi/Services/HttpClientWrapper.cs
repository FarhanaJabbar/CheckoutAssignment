using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CheckoutApi.Controllers;
using CheckoutApi.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CheckoutApi.Services
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private static readonly HttpClient HttpClient = new();

        public async Task<HttpWrappedResponse<T>> GetAsync<T>(string requestUri, HttpMethod method, CancellationToken cancellationToken = default, object requestBody = null, Dictionary<string, string> customRequestHeaders = null, ContentType contentType = ContentType.Json)
        {
            try
            {
                var request = CreateRequest(requestUri, method, requestBody, customRequestHeaders, contentType);
                var response = await GetResponseAsync(request, cancellationToken);
                if (!response.IsSuccessStatusCode)
                    throw new ApiException($"Content Error: {response.StatusCode}", null, response);
                var content = response.Content.ReadAsStringAsync(cancellationToken).Result;
                var data = JsonConvert.DeserializeObject<T>(content);
                return new HttpWrappedResponse<T>(data, response);

            }
            catch (Exception ex)
            {
                if (ex is ApiException) throw;
                throw new ApiException($"Error occurred in executing request: {requestUri}", ex);
            }
        }
        
        private static async Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage request, CancellationToken token)
        {
            var response = await HttpClient.SendAsync(request, token).ConfigureAwait(false);
            return response;
        }


        private static HttpRequestMessage CreateRequest(string requestUri, HttpMethod method, object requestBody, Dictionary<string, string> customRequestHeaders , ContentType contentType)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            switch (contentType)
            {
                case ContentType.FormUrlEncodedContent:
                   
                    if (requestBody is IDictionary<string,string>  requestParams)
                    {
                        request.Headers.Add("Accept", "application/x-www-form-urlencoded");
                        var content = new FormUrlEncodedContent(requestParams);
                        request.Content = content;
                    }
                    break;
                default:
                  
                    if (requestBody != null)
                    {
                        request.Headers.Add("Accept", "application/json");
                        var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                        request.Content = content;
                    }

                    break;
            }
          

            if (customRequestHeaders != null)
                foreach (var (key, value) in customRequestHeaders) request.Headers.Add(key, value);

            request.Method = method;
            return request;
        }
    }
}
