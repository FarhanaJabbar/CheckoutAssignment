using System;
using System.Threading.Tasks;
using CheckoutApi.Models;
using CheckoutApi.Services;
using Moq;
using Xunit;

namespace Checkout.UnitTests
{
    public class TransactionServiceTests
    {
        private readonly Mock<ITransactionRepository> _transactionRepository = new();
        private readonly Mock<IEncryptionService> _encryptionService = new() ;
        private readonly Mock<IBankingService> _bankingService= new();
        private readonly Mock<ILoggerWrapper<TransactionService>> _logger = new();

        [Fact]
        public async Task Should_Return_Success_Response_For_Successful_Payment_With_Save_Card()
        {
            _bankingService.Setup(x => x.MakeTransactionAsync(It.IsAny<TransactionRequest>())).ReturnsAsync(new TransactionResponse() {StatusCode = "1000", Success = true, TransactionReference = Guid.NewGuid().ToString()});
            var transactionService = new TransactionService(_transactionRepository.Object, _bankingService.Object, _logger.Object, _encryptionService.Object);
            var response = await transactionService.MakePaymentAsync(new TransactionRequest(){SaveCardDetails = true});
            _encryptionService.Verify(x=> x.EncryptString(It.IsAny<string>()), Times.Exactly(11));
            _transactionRepository.Verify(x=> x.AddTransactionAsync(It.IsAny<Transaction>()), Times.Once);
            Assert.True(response.Success);
            Assert.NotEmpty(response.TransactionReference);
        }

        [Fact]
        public async Task Should_Return_Success_Response_For_Successful_Payment_Without_Save_Card()
        {
            _bankingService.Setup(x => x.MakeTransactionAsync(It.IsAny<TransactionRequest>())).ReturnsAsync(new TransactionResponse() { StatusCode = "1000", Success = true, TransactionReference = Guid.NewGuid().ToString() });
            var transactionService = new TransactionService(_transactionRepository.Object, _bankingService.Object, _logger.Object, _encryptionService.Object);
            var response = await transactionService.MakePaymentAsync(new TransactionRequest());
            _encryptionService.Verify(x => x.EncryptString(It.IsAny<string>()), Times.Exactly(6));
            _transactionRepository.Verify(x => x.AddTransactionAsync(It.IsAny<Transaction>()), Times.Once);
            Assert.True(response.Success);
            Assert.NotEmpty(response.TransactionReference);
        }

        [Fact]
        public async Task Should_Return_Failed_Response_For_Failed_Payment()
        {
            _bankingService.Setup(x => x.MakeTransactionAsync(It.IsAny<TransactionRequest>())).ReturnsAsync(new TransactionResponse() { StatusCode = "2000", Success = false });
            var transactionService = new TransactionService(_transactionRepository.Object, _bankingService.Object, _logger.Object, _encryptionService.Object);
            var response = await transactionService.MakePaymentAsync(new TransactionRequest());
            _encryptionService.Verify(x => x.EncryptString(It.IsAny<string>()), Times.Never);
            _transactionRepository.Verify(x => x.AddTransactionAsync(It.IsAny<Transaction>()), Times.Never);
            Assert.False(response.Success);
            Assert.Null(response.TransactionReference);
        }

        [Fact]
        public async Task Should_Log_Error_For_System_Error_On_MakePaymentAsync()
        {
            _bankingService.Setup(x => x.MakeTransactionAsync(It.IsAny<TransactionRequest>())).ThrowsAsync(new Exception());
            var transactionService = new TransactionService(_transactionRepository.Object, _bankingService.Object, _logger.Object, _encryptionService.Object);
             await Assert.ThrowsAsync<Exception>(()=> transactionService.MakePaymentAsync(new TransactionRequest()));
            _encryptionService.Verify(x => x.EncryptString(It.IsAny<string>()), Times.Never);
            _transactionRepository.Verify(x => x.AddTransactionAsync(It.IsAny<Transaction>()), Times.Never);
            _logger.Verify(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
        }

        [Fact]
        public async Task Should_Return_Null_For_Invalid_PaymentReference()
        {
            var transactionService = new TransactionService(_transactionRepository.Object, _bankingService.Object, _logger.Object, _encryptionService.Object);
            _transactionRepository.Setup(x => x.GetTransactionAsync(It.IsAny<string>())).ReturnsAsync(() => null);
            var response = await transactionService.GeTransactionDetailAsync("234234");
            _encryptionService.Verify(x => x.DecryptString(It.IsAny<string>()), Times.Never);
            _transactionRepository.Verify(x => x.GetTransactionAsync(It.IsAny<string>()), Times.Once());
            Assert.Null(response);
        }


        [Fact]
        public async Task Should_Return_TransactionDetails_For_Valid_PaymentReference_With_SavedCard_Details()
        {
            var transactionService = new TransactionService(_transactionRepository.Object, _bankingService.Object, _logger.Object, _encryptionService.Object);
            _transactionRepository.Setup(x => x.GetTransactionAsync(It.IsAny<string>())).ReturnsAsync(() => new Transaction(){CardNumber = "32423423", CardHolderName = "Encrypted Data"});
            _encryptionService.Setup(x => x.DecryptString(It.IsAny<string>())).Returns("234234234");

            var response = await transactionService.GeTransactionDetailAsync("234234");
           
            Assert.NotNull(response);
            _transactionRepository.Verify(x => x.GetTransactionAsync(It.IsAny<string>()), Times.Once());
            _encryptionService.Verify(x => x.DecryptString(It.IsAny<string>()), Times.Exactly(8));
            Assert.Equal(4, response.LastFourDigitsOfCardNumber.Length);

        }

        [Fact]
        public async Task Should_Return_TransactionDetails_For_Valid_PaymentReference_Without_SavedCard_Details()
        {
            var transactionService = new TransactionService(_transactionRepository.Object, _bankingService.Object, _logger.Object, _encryptionService.Object);
            _transactionRepository.Setup(x => x.GetTransactionAsync(It.IsAny<string>())).ReturnsAsync(() => new Transaction());
            _encryptionService.Setup(x => x.DecryptString(It.IsAny<string>())).Returns("234234234");

            var response = await transactionService.GeTransactionDetailAsync("234234");

            Assert.NotNull(response);
            _transactionRepository.Verify(x => x.GetTransactionAsync(It.IsAny<string>()), Times.Once());
            _encryptionService.Verify(x => x.DecryptString(It.IsAny<string>()), Times.Exactly(6));

        }

        [Fact]
        public async Task Should_Log_Error_For_System_Error_On_GeTransactionDetailAsync()
        {
            _transactionRepository.Setup(x => x.GetTransactionAsync(It.IsAny<string>())).ThrowsAsync(new Exception());
            var transactionService = new TransactionService(_transactionRepository.Object, _bankingService.Object, _logger.Object, _encryptionService.Object);
            await Assert.ThrowsAsync<Exception>(() => transactionService.GeTransactionDetailAsync("23423423"));
            _encryptionService.Verify(x => x.DecryptString(It.IsAny<string>()), Times.Never);
            _logger.Verify(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
        }

    }
}
