using System;
using System.Collections.Generic;
using CheckoutApi.Models;
using CheckoutApi.Validators;
using Xunit;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Azure;

namespace Checkout.UnitTests
{
    public class TransactionRequestValidationTests
    {
        private readonly TransactionRequestValidator _transactionRequestValidator;
        public TransactionRequestValidationTests()
        {
            _transactionRequestValidator = new TransactionRequestValidator();
        }

        [Theory]
        [MemberData(nameof(GetEmptyDataObject))]
        public void Validation_Should_Failed_For_Required_Fields(TransactionRequest model)
        {
            var result = _transactionRequestValidator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(request => request.CardNumber);
            result.ShouldHaveValidationErrorFor(request => request.Amount);
            result.ShouldHaveValidationErrorFor(request => request.CardHolderName);
            result.ShouldHaveValidationErrorFor(request => request.CardExpiryMonth);
            result.ShouldHaveValidationErrorFor(request => request.CardExpiryYear);
            result.ShouldHaveValidationErrorFor(request => request.SecurityCode);
            result.ShouldHaveValidationErrorFor(request => request.CurrencyCode);
            result.ShouldHaveValidationErrorFor(request => request.BillingAddressLine1);
            result.ShouldHaveValidationErrorFor(request => request.BillingAddressPostcode);
            result.ShouldHaveValidationErrorFor(request => request.BillingAddressCountry);

        }


        [Theory]
        [MemberData(nameof(GetDataObjectWithInvalidDataFormats))]
        public void Validation_Should_Failed_For_Invalid_Data_Format(TransactionRequest model)
        {
            var result = _transactionRequestValidator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(request => request.SecurityCode);
            result.ShouldHaveValidationErrorFor(request => request.CardNumber);

        }

        [Theory]
        [MemberData(nameof(GetDataObjectWithInvalidDataSize))]
        public void Validation_Should_Failed_For_Invalid_Data_Size(TransactionRequest model)
        {
            var result = _transactionRequestValidator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(request => request.CardHolderName);
            result.ShouldHaveValidationErrorFor(request => request.CurrencyCode);
            result.ShouldHaveValidationErrorFor(request => request.BillingAddressLine1);
            result.ShouldHaveValidationErrorFor(request => request.BillingAddressLine2);
            result.ShouldHaveValidationErrorFor(request => request.BillingAddressCity);
            result.ShouldHaveValidationErrorFor(request => request.BillingAddressCounty);
            result.ShouldHaveValidationErrorFor(request => request.BillingAddressPostcode);
            result.ShouldHaveValidationErrorFor(request => request.BillingAddressCountry);

        }


        [Theory]
        [MemberData(nameof(GetDataObjectWithMinimumRequiredData))]
        public void Validation_Should_Pass_For_Minimum_Required_Data(TransactionRequest model)
        {
            var result = _transactionRequestValidator.TestValidate(model);
            Assert.True(result.IsValid);
        }

        [Theory]
        [MemberData(nameof(GetDataObjectWithValidCardNumbers))]
        public void Validation_Should_Pass_For_Valid_Card_Number(TransactionRequest model)
        {
            var result = _transactionRequestValidator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(request => request.CardNumber);
        }

        [Fact]
        public void Should_Fail_Validation_For_CardExpiryDate()
        {
            var model = new TransactionRequest {CardExpiryMonth = DateTime.Now.AddMonths(-1).Month, CardExpiryYear = DateTime.Now.Year};
            var result = _transactionRequestValidator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(request => request.CardExpiryMonth);
            result.ShouldNotHaveValidationErrorFor(request => request.CardExpiryYear);


            model = new TransactionRequest { CardExpiryMonth = DateTime.Now.AddMonths(1).Month, CardExpiryYear = DateTime.Now.AddYears(-1).Year };
            result = _transactionRequestValidator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(request => request.CardExpiryMonth);
            result.ShouldHaveValidationErrorFor(request => request.CardExpiryYear);
        }

        [Fact]
        public void Should_Pass_Validation_For_CardExpiryDate()
        {
            var model = new TransactionRequest { CardExpiryMonth = DateTime.Now.Month , CardExpiryYear = DateTime.Now.Year };
            var result = _transactionRequestValidator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(request => request.CardExpiryMonth);
            result.ShouldNotHaveValidationErrorFor(request => request.CardExpiryYear);


            model = new TransactionRequest { CardExpiryMonth = DateTime.Now.AddMonths(3).Month, CardExpiryYear = DateTime.Now.AddYears(1).Year };
            result = _transactionRequestValidator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(request => request.CardExpiryMonth);
            result.ShouldNotHaveValidationErrorFor(request => request.CardExpiryYear);
        }


        #region Build Test Data
        public static IEnumerable<object[]> GetEmptyDataObject()
        {
            var allData = new List<object[]>
            {
                new object[]{ new TransactionRequest()}
            };

            return allData;
        }

        public static IEnumerable<object[]> GetDataObjectWithInvalidDataFormats()
        {
            var allData = new List<object[]>
            {
                new object[]{ new TransactionRequest
                {
                    SecurityCode = "1234",
                    CardNumber = "2342-34234 2342-3423"
    
                }}
            };

            return allData;
        }

        public static IEnumerable<object[]> GetDataObjectWithValidCardNumbers()
        {
            var allData = new List<object[]>
            {
                new object[]{ new TransactionRequest
                {
                    CardNumber = "2221-0000-0000-0009" //Master Card
                }},
                new object[]{ new TransactionRequest
                {
                    CardNumber = "5555 5555 5555 4444" //Master Card
                }},
                new object[]{ new TransactionRequest
                {
                    CardNumber = "5105105105105100" //Master Card
                }},
                new object[]{ new TransactionRequest
                {
                    CardNumber = "4111111111111111" //Visa
                }},
                new object[]{ new TransactionRequest
                {
                    CardNumber = "4012888888881881" //Visa
                }},
                new object[]{ new TransactionRequest
                {
                    CardNumber = "4222222222222" //Visa
                }},
                new object[]{ new TransactionRequest
                {
                    CardNumber = "378282246310005" //American Express
                }},
                new object[]{ new TransactionRequest
                {
                    CardNumber = "371449635398431" //American Express
                }},
                new object[]{ new TransactionRequest
                {
                    CardNumber = "378734493671000" //American Express Corporate
                }},
                new object[]{ new TransactionRequest
                {
                    CardNumber = "30569309025904" //Diners Club
                }},
                new object[]{ new TransactionRequest
                {
                    CardNumber = "6011111111111117" //Discover
                }},
                new object[]{ new TransactionRequest
                {
                    CardNumber = "3530111333300000" //JCB
                }}
            };

            return allData;
        }

        public static IEnumerable<object[]> GetDataObjectWithMinimumRequiredData()
        {
            var allData = new List<object[]>
            {
                new object[]{ new TransactionRequest
                {
                    CardHolderName = "Farhana",
                    Amount = 1000,
                    CurrencyCode = "GBP",
                    SecurityCode = "123",
                    CardNumber = "2221-0000-0000-0009",
                    CardExpiryMonth = DateTime.Now.AddMonths(1).Month,
                    CardExpiryYear = DateTime.Now.AddYears(2).Year,
                    BillingAddressLine1 = "34 sdfsd sdfsdf",
                    BillingAddressPostcode = "sdfsdf",
                    BillingAddressCountry = "sdfsdf"
                }}
            };

            return allData;
        }

        public static IEnumerable<object[]> GetDataObjectWithInvalidDataSize()
        {
            var allData = new List<object[]>
            {
                new object[]{ new TransactionRequest
                {
                    CurrencyCode = "ASDASDASDASD",
                    CardHolderName = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque",
                    BillingAddressCity = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque,  Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque",
                    BillingAddressCountry = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque,  Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque",
                    BillingAddressCounty = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque,  Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque",
                    BillingAddressLine1 = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque,  Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque",
                    BillingAddressLine2 = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque,  Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque",
                    BillingAddressPostcode = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque,  Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque",
                   
                }}
            };

            return allData;
        }
#endregion
    }
}
