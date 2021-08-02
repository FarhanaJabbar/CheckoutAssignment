using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CheckoutApi.Models;
using Microsoft.Extensions.Options;

namespace CheckoutApi.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IOptions<AuthSettings> _authSettingsOptions;
        private readonly IHttpClientWrapper _httpClientWrapper;
        private readonly ILoggerWrapper<AuthenticationService> _logger;
        public AuthenticationService(IOptions<AuthSettings> authSettings, IHttpClientWrapper httpClientWrapper, ILoggerWrapper<AuthenticationService> logger)
        {
            this._authSettingsOptions = authSettings;
            _httpClientWrapper = httpClientWrapper;
            _logger = logger;
        }

        public async Task<CheckoutTokenResponse> GetToken(string userName, string password)
        {
            try
            {
                var requestParams = new Dictionary<string, string>
                {
                    {"grant_type", "password"},
                    {"client_id", _authSettingsOptions.Value.ClientId},
                    {"client_secret", _authSettingsOptions.Value.ClientSecret},
                    {"username", userName},
                    {"password", password},
                    {"audience", _authSettingsOptions.Value.Audience}
                };

                var response = await _httpClientWrapper.GetAsync<Auth0TokenResponse>(_authSettingsOptions.Value.TokenUrl,
                    HttpMethod.Post,
                    requestBody: requestParams, contentType: ContentType.FormUrlEncodedContent);
                if (!response.Success) throw new Exception($"Error Getting Token, {response.StatusCode}, { await response.RawResponse.Content.ReadAsStringAsync()}");
                return new CheckoutTokenResponse()
                {
                    AccessToken = response.Data.AccessToken,
                    ExpiresIn = response.Data.ExpiresIn,
                    Scope = response.Data.Scope,
                    TokenType = response.Data.TokenType
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }
    }
}
