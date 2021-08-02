using System;

namespace CheckoutApi.Models
{
    public class TransactionDetailResponse
    {
        public string Reference { get; set; }
        public string StatusCode { get; set; }
        public DateTime Date { get; set; }
        
        public string LastFourDigitsOfCardNumber { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Amount { get; set; }
        public string CardHolderName { get; set; }
        public string BillingAddressLine1 { get; set; }
        public string BillingAddressLine2 { get; set; }
        public string BillingAddressCity { get; set; }
        public string BillingAddressCounty { get; set; }
        public string BillingAddressPostcode { get; set; }
        public string BillingAddressCountry { get; set; }
    }
}
