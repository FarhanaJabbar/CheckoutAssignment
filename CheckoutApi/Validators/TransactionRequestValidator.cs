using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CheckoutApi.Models;
using FluentValidation;

namespace CheckoutApi.Validators
{
    public class TransactionRequestValidator : AbstractValidator<TransactionRequest>
    {

        public TransactionRequestValidator()
        {
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Transaction Amount is required");
            RuleFor(x => x.CardNumber).NotEmpty().WithMessage("Card number is required").MaximumLength(20);
            RuleFor(x => x.CardNumber).Must(IsValidCard).WithMessage("Invalid Card number").When(x => !string.IsNullOrEmpty(x.CardNumber));
            RuleFor(x => x.SecurityCode).NotEmpty().WithMessage("Security code required").MaximumLength(3).Matches("[0-9]{3,3}").WithMessage("Invalid Security Code");
            RuleFor(x => x.CurrencyCode).NotEmpty().WithMessage("Currency code required").MaximumLength(3);
            RuleFor(x => x.CardExpiryYear).GreaterThanOrEqualTo(DateTime.Now.Year).WithMessage("Year should not be past year");
            RuleFor(x => x.CardExpiryMonth).Must((month)=> month>=1 && month<=12 ).WithMessage("Month value should between 1 and 12");
            RuleFor(x => x.CardExpiryMonth).Must((x)=> x >= DateTime.Now.Month).When(x=> x.CardExpiryYear == DateTime.Now.Year).WithMessage("Month should be a future month");
            
            RuleFor(x => x.CardHolderName).NotEmpty().WithMessage("Card Holder Name is required").MaximumLength(100);
            RuleFor(x => x.BillingAddressLine1).NotEmpty().WithMessage("Billing address Line 1 is required").MaximumLength(100);
            RuleFor(x => x.BillingAddressPostcode).NotEmpty().WithMessage("Billing address Postcode is required").MaximumLength(20);
            RuleFor(x => x.BillingAddressCountry).NotEmpty().WithMessage("Billing Address Country is required").MaximumLength(50);
            RuleFor(x => x.BillingAddressCity).MaximumLength(50);
            RuleFor(x => x.BillingAddressLine2).MaximumLength(100);
            RuleFor(x => x.BillingAddressCounty).MaximumLength(50);


        }

        private bool IsValidCard(string cardNumber)
        {
            cardNumber = Regex.Replace(cardNumber, @"[\s\-]+", "", RegexOptions.IgnoreCase);

            var validCardPattern = "^(?:4[0-9]{12}(?:[0-9]{3})?" // Visa
                                   + "|(?:5[1-5][0-9]{2}"
                                   + "|222[1-9]|22[3-9][0-9]|2[3-6][0-9]{2}|27[01][0-9]|2720)[0-9]{12}" // Master Card
                                   + "|3[47][0-9]{13}" // American Express
                                   + "|3(?:0[0-5]|[68][0-9])[0-9]{11}" // Diners Club
                                   + "|6(?:011|5[0-9]{2})[0-9]{12}" // Discover
                                   + @"|(?:2131|1800|35\d{3})\d{11})$"; // JCB;
            var isValid = Regex.IsMatch(cardNumber, validCardPattern, RegexOptions.IgnoreCase);

            return isValid;
        }
    }
}
