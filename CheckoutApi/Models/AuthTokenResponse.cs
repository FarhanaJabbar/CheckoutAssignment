using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CheckoutApi.Models
{
    public class Auth0TokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        public string Scope { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }

    public class CheckoutTokenResponse 
    {
        public string AccessToken { get; set; }

        public string TokenType { get; set; }
        public string Scope { get; set; }

        public int ExpiresIn { get; set; }
    }
}
