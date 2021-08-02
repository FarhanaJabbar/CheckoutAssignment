using System;
using System.Threading.Tasks;
using CheckoutApi.Models;

namespace CheckoutApi.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly IBankingService _bankingService;
        private readonly ILoggerWrapper<TransactionService> _logger;
        public TransactionService(ITransactionRepository transactionRepository, IBankingService bankingService, ILoggerWrapper<TransactionService> logger, IEncryptionService encryptionService)
        {
            _transactionRepository = transactionRepository;
            _bankingService = bankingService;
            _logger = logger;
            _encryptionService = encryptionService;
        }

        public async Task<TransactionResponse> MakePaymentAsync(TransactionRequest transactionRequest)
        {
            try
            {
               var result = await _bankingService.MakeTransactionAsync(transactionRequest);
               if (!result.Success) return result;
               var transactionDetail = new Transaction
               {
                   Id = Guid.NewGuid(),
                   Reference = result.TransactionReference,
                   StatusCode = result.StatusCode,
                   CardNumber = transactionRequest.SaveCardDetails? _encryptionService.EncryptString(transactionRequest.CardNumber) : string.Empty,
                   SecurityCode = transactionRequest.SaveCardDetails ? _encryptionService.EncryptString(transactionRequest.SecurityCode) : string.Empty,
                   CardHolderName = transactionRequest.SaveCardDetails ? _encryptionService.EncryptString(transactionRequest.CardHolderName) :string.Empty,
                   CardExpiryYear = transactionRequest.SaveCardDetails ? _encryptionService.EncryptString(transactionRequest.CardExpiryYear.ToString()) : string.Empty,
                   CardExpiryMonth = transactionRequest.SaveCardDetails ? _encryptionService.EncryptString(transactionRequest.CardExpiryMonth.ToString()): string.Empty,
                   CurrencyCode = transactionRequest.CurrencyCode,
                   Amount = transactionRequest.Amount,
                   Date = DateTime.Now,
                   BillingAddressLine1 = _encryptionService.EncryptString(transactionRequest.BillingAddressLine1),
                   BillingAddressLine2 = _encryptionService.EncryptString(transactionRequest.BillingAddressLine2),
                   BillingAddressCity = _encryptionService.EncryptString(transactionRequest.BillingAddressCity),
                   BillingAddressCountry = _encryptionService.EncryptString(transactionRequest.BillingAddressCountry),
                   BillingAddressCounty = _encryptionService.EncryptString(transactionRequest.BillingAddressCounty),
                   BillingAddressPostcode = _encryptionService.EncryptString(transactionRequest.BillingAddressPostcode)
               };

               await _transactionRepository.AddTransactionAsync(transactionDetail);
               return result;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex); 
                throw;
            }
        }

        public async Task<TransactionDetailResponse> GeTransactionDetailAsync(string transactionReference)
        {
            try
            {
                var transactionDetail = await _transactionRepository.GetTransactionAsync(transactionReference);
                if (transactionDetail == null) return null;

                
                var cardNumber =!string.IsNullOrEmpty(transactionDetail.CardNumber) ? _encryptionService.DecryptString(transactionDetail.CardNumber) : null;
                if (cardNumber?.Length > 4)
                    cardNumber = cardNumber.Substring(cardNumber.Length - 4);
                return new TransactionDetailResponse()
                {
                    Reference = transactionDetail.Reference,
                    StatusCode = transactionDetail.StatusCode,
                    LastFourDigitsOfCardNumber = cardNumber,
                    CardHolderName = !string.IsNullOrEmpty(transactionDetail.CardHolderName)? _encryptionService.DecryptString(transactionDetail.CardHolderName) : null,
                    CurrencyCode = transactionDetail.CurrencyCode,
                    Amount = transactionDetail.Amount,
                    Date = transactionDetail.Date,
                    BillingAddressLine1 = _encryptionService.DecryptString(transactionDetail.BillingAddressLine1),
                    BillingAddressLine2 = _encryptionService.DecryptString(transactionDetail.BillingAddressLine2),
                    BillingAddressCity = _encryptionService.DecryptString(transactionDetail.BillingAddressCity),
                    BillingAddressCountry = _encryptionService.DecryptString(transactionDetail.BillingAddressCountry),
                    BillingAddressCounty = _encryptionService.DecryptString(transactionDetail.BillingAddressCounty),
                    BillingAddressPostcode = _encryptionService.DecryptString(transactionDetail.BillingAddressPostcode)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

    }
}
