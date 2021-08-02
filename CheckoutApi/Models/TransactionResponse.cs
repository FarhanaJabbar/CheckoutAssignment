using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutApi.Models
{
    public class TransactionResponse
    {
        public bool Success { get; set; }
        public string TransactionReference { get; set; }
        public string StatusCode { get; set; }
    }
}
