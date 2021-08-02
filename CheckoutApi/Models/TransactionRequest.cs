using System;
using System.ComponentModel.DataAnnotations;

namespace CheckoutApi.Models
{
    public class TransactionRequest
    {
        public string CardNumber { get; set; }
      
        public int CardExpiryMonth { get; set; }

        public int CardExpiryYear { get; set; }

        public string SecurityCode { get; set; }
        public string CurrencyCode { get; set; }

        public decimal Amount { get; set; }

        public string CardHolderName { get; set; }

        public string BillingAddressLine1 { get; set; }

        public string BillingAddressLine2 { get; set; }
    
        public string BillingAddressCity { get; set; }

        public string BillingAddressCounty { get; set; }

        public string BillingAddressPostcode { get; set; }

        public string BillingAddressCountry { get; set; }

        public bool SaveCardDetails { get; set; }

    }
}
