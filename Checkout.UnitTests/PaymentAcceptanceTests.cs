using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CheckoutApi.Models;
using CheckoutApi.Services;
using Xunit;

namespace Checkout.UnitTests
{
    //NOTE Please Run IISExpress without debuging mode before running these acceptance TESTS
    public class PaymentAcceptanceTests
    {
        private const string _apiBaseUrl = "https://localhost:44306/api";
        private HttpClientWrapper httpClientWrapper = new();

        [Fact]
        public async Task Given_Valid_Card_Details_When_I_Make_Payment_Then_I_Should_Get_Success_Payment_Response()
        {
            var tokenResponse = await GetTokenAsync();
            var transactionRequest = new TransactionRequest()
            {
                Amount = 1000,
                CardExpiryYear = DateTime.Now.AddYears(2).Year,
                CardExpiryMonth = DateTime.Now.Month,
                CardNumber = "5555555555554444",
                CardHolderName = "Test User",
                SecurityCode = "123",
                CurrencyCode = "GBP",
                BillingAddressLine1 = "sdsdf sdf sdf",
                BillingAddressPostcode = "E11 4TY",
                BillingAddressCountry = "UK",
                SaveCardDetails = true
            };

           var makePaymentResponse = await MakePaymentAsync(transactionRequest, tokenResponse);

            Assert.True(makePaymentResponse.Success);
            Assert.NotNull(makePaymentResponse.Data);
            Assert.NotNull(makePaymentResponse.Data.TransactionReference);
        }

        private async Task<HttpWrappedResponse<TransactionResponse>> MakePaymentAsync(TransactionRequest transactionRequest, HttpWrappedResponse<CheckoutTokenResponse> tokenResponse)
        {
            var makePaymentResponse = await httpClientWrapper.GetAsync<TransactionResponse>($"{_apiBaseUrl}/payment",
                HttpMethod.Post, CancellationToken.None, 
                requestBody: transactionRequest,customRequestHeaders: tokenResponse!=null? new Dictionary<string, string>() { { "Authorization", $"Bearer  {tokenResponse.Data.AccessToken}" } } : null);

            return makePaymentResponse;
        }

        private async Task<HttpWrappedResponse<TransactionDetailResponse>> GetPaymentDetailAsync(string transactionReference, HttpWrappedResponse<CheckoutTokenResponse> tokenResponse)
        {
            var paymentDetailResponse = await httpClientWrapper.GetAsync<TransactionDetailResponse>($"{_apiBaseUrl}/payment/{transactionReference}",
                HttpMethod.Get, 
                customRequestHeaders: tokenResponse != null ? new Dictionary<string, string>() { { "Authorization", $"Bearer  {tokenResponse.Data.AccessToken}" } } : null);

            return paymentDetailResponse;
        }

        [Fact]
        public async Task Given_Valid_PaymentReference_When_I_Request_Payment_Details_Then_I_Should_Get_Correct_Payment_Details()
        {
            var tokenResponse = await GetTokenAsync();
            var transactionRequest = new TransactionRequest()
            {
                Amount = 1000,
                CardExpiryYear = DateTime.Now.AddYears(2).Year,
                CardExpiryMonth = DateTime.Now.Month,
                CardNumber = "5555555555554444",
                CardHolderName = "Test User",
                SecurityCode = "123",
                CurrencyCode = "GBP",
                BillingAddressLine1 = "sdsdf sdf sdf",
                BillingAddressPostcode = "E11 4TY",
                BillingAddressCountry = "UK",
                SaveCardDetails = true
            };
            var makePaymentResponse = await MakePaymentAsync(transactionRequest, tokenResponse);

            var paymentDetail =
                await GetPaymentDetailAsync(makePaymentResponse.Data.TransactionReference, tokenResponse);

            Assert.True(paymentDetail.Success);
            Assert.NotNull(paymentDetail.Data);
            Assert.Equal(paymentDetail.Data.Amount , transactionRequest.Amount);
            Assert.Equal(paymentDetail.Data.CurrencyCode, transactionRequest.CurrencyCode);
            Assert.Equal(paymentDetail.Data.Reference, makePaymentResponse.Data.TransactionReference);
            Assert.Equal("4444", paymentDetail.Data.LastFourDigitsOfCardNumber);

        }


        [Fact]
        public async Task Given_Invalid_PaymentReference_When_I_Request_Payment_Details_Then_I_Should_Get_Not_Found_Response()
        {
            var tokenResponse = await GetTokenAsync();
           
            var error =
               await Assert.ThrowsAsync<ApiException>(()=>GetPaymentDetailAsync("invalid payment reference", tokenResponse));
            Assert.Equal(HttpStatusCode.NotFound, error.ResponseMessage.StatusCode);


        }

        [Fact]
        public async Task Given_Invalid_TransactionRequest_When_I_Request_MakePayment_Then_I_Should_Get_BadRequest_Response()
        {
            var tokenResponse = await GetTokenAsync();

            var error =
                await Assert.ThrowsAsync<ApiException>(() => MakePaymentAsync(new TransactionRequest(), tokenResponse));
            Assert.Equal(HttpStatusCode.BadRequest, error.ResponseMessage.StatusCode);
        }

        [Fact]
        public async Task Given_No_Api_Token_When_I_Request_Payment_Then_I_Should_Get_UnAuthorizeResponse()
        {
            
            var error =
                await Assert.ThrowsAsync<ApiException>(() => MakePaymentAsync(new TransactionRequest(), null));
            Assert.Equal(HttpStatusCode.Unauthorized, error.ResponseMessage.StatusCode);
        }

        [Fact]
        public async Task Given_No_Api_Token_When_I_Request_Payment_Details_Then_I_Should_Get_UnAuthorizeResponse()
        {

            var error =
                await Assert.ThrowsAsync<ApiException>(() => GetPaymentDetailAsync("aasdasd", null));
            Assert.Equal(HttpStatusCode.Unauthorized, error.ResponseMessage.StatusCode);
        }

        private async Task<HttpWrappedResponse<CheckoutTokenResponse>> GetTokenAsync()
        {
            var tokenResponse = await httpClientWrapper.GetAsync<CheckoutTokenResponse>($"{_apiBaseUrl}/token", HttpMethod.Post, requestBody: new TokenRequest { UserName = "Farhana.jabbar", Password = "Password123!" });
            Assert.True(tokenResponse.Success);
            Assert.NotNull(tokenResponse.Data.AccessToken);
            return tokenResponse;
        }
    }
}
