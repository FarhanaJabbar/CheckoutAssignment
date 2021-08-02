using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutApi.Models
{
    public class AuthSettings
    {
        public string Authority { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string TokenUrl { get; set; }
        public string Audience { get; set; }
    }
}
