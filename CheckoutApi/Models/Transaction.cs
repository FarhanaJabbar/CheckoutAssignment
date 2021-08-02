using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckoutApi.Models
{
    [Table("Transaction")]
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; } 

        [Required]
        [MaxLength(50)]
        public string Reference { get; set; } //Bank Reference
        [Required]
        [MaxLength(20)]
        public string StatusCode { get; set; }
        [Required]
        public DateTime Date { get; set; }

       
        [MaxLength(200)] //because we will encrypt this value
        public string CardNumber { get; set; }

      
        [MaxLength(200)] //because we will encrypt this value
        public string SecurityCode { get; set; }

      
        [Required]
        [MaxLength(200)]
        public string CurrencyCode { get; set; }

      
        [MaxLength(200)]
        public string CardExpiryMonth { get; set; }

        
        [MaxLength(200)]
        public string CardExpiryYear { get; set; }

        [Required]
        public decimal Amount { get; set; }

       
        [MaxLength(400)]
        public string CardHolderName { get; set; }


        [Required]
        [MaxLength(400)]
        public string BillingAddressLine1 { get; set; }

        [MaxLength(200)]
        public string BillingAddressLine2 { get; set; }
        [MaxLength(200)]
        public string BillingAddressCity { get; set; }
        [MaxLength(200)]
        public string BillingAddressCounty { get; set; }

        [Required]
        [MaxLength(200)]
        public string BillingAddressPostcode { get; set; }

        [Required]
        [MaxLength(200)]
        public string BillingAddressCountry { get; set; } 
    }

}
