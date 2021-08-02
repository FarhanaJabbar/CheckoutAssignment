using System;
using System.Threading.Tasks;
using CheckoutApi.Database;
using CheckoutApi.Models;
using CheckoutApi.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Checkout.UnitTests
{
    public class TransactionRepositoryTests
    {
        private readonly Mock<IConfiguration> _configuration = new();
      

        public TransactionRepositoryTests()
        {
            var mockConfSection = new Mock<IConfigurationSection>();
            mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "CheckoutDb")]).Returns("Server=localhost;Database=CheckoutDB;Trusted_Connection=True;");

            _configuration.Setup(a => a.GetSection(It.Is<string>(s => s == "ConnectionStrings"))).Returns(mockConfSection.Object);
            
        }

        [Fact]
        public async Task Should_Add_New_Transaction_Verify_Saved_Transaction_Delete_Saved_Transaction()
        {
            var checkoutDbContext = new CheckoutDbContext(_configuration.Object);
            var transactionRepository = new TransactionRepository(checkoutDbContext);
            var transactionId = Guid.NewGuid();
            var transactionToAdd = new Transaction
            {
                Reference = transactionId.ToString(),
                Id = transactionId,
                CardHolderName = "Encrypted Data",
                CardExpiryYear = "Encrypted Data",
                CardExpiryMonth = "Encrypted Data",
                StatusCode = "1000",
                Amount = 1000,
                CurrencyCode = "GBP",
                SecurityCode = "Encrypted Data",
                CardNumber = "Encrypted Data",
                Date = DateTime.Now,
                BillingAddressLine1 = "Encrypted Data",
                BillingAddressLine2 = "Encrypted Data",
                BillingAddressCity = "Encrypted Data",
                BillingAddressCounty = "Encrypted Data",
                BillingAddressPostcode = "Encrypted Data",
                BillingAddressCountry = "Encrypted Data"
            };

           await transactionRepository.AddTransactionAsync(transactionToAdd);

           var savedTransaction =await  transactionRepository.GetTransactionAsync(transactionId.ToString());

           Assert.Equal(transactionToAdd.Reference, savedTransaction.Reference);
           Assert.Equal(transactionToAdd.Id, savedTransaction.Id);
           Assert.Equal(transactionToAdd.CardHolderName, savedTransaction.CardHolderName);
           Assert.Equal(transactionToAdd.CardExpiryYear, savedTransaction.CardExpiryYear);
           Assert.Equal(transactionToAdd.CardExpiryMonth, savedTransaction.CardExpiryMonth);
            Assert.Equal(transactionToAdd.StatusCode, savedTransaction.StatusCode);
           Assert.Equal(transactionToAdd.Amount, savedTransaction.Amount);
           Assert.Equal(transactionToAdd.CurrencyCode, savedTransaction.CurrencyCode);
           Assert.Equal(transactionToAdd.SecurityCode, savedTransaction.SecurityCode);
           Assert.Equal(transactionToAdd.CardNumber, savedTransaction.CardNumber);
           Assert.Equal(transactionToAdd.Date, savedTransaction.Date);
           Assert.Equal(transactionToAdd.BillingAddressLine1, savedTransaction.BillingAddressLine1);
           Assert.Equal(transactionToAdd.BillingAddressLine2, savedTransaction.BillingAddressLine2);
           Assert.Equal(transactionToAdd.BillingAddressCity, savedTransaction.BillingAddressCity);
           Assert.Equal(transactionToAdd.BillingAddressCounty, savedTransaction.BillingAddressCounty);
           Assert.Equal(transactionToAdd.BillingAddressPostcode, savedTransaction.BillingAddressPostcode);
           Assert.Equal(transactionToAdd.BillingAddressCountry, savedTransaction.BillingAddressCountry);


           await transactionRepository.DeleteTransactionAsync(savedTransaction.Id);

           savedTransaction = await transactionRepository.GetTransactionAsync(transactionId.ToString());

           Assert.Null(savedTransaction);

        }

    }
}
