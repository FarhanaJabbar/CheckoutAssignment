using System;
using System.Threading.Tasks;
using CheckoutApi.Models;

namespace CheckoutApi.Services
{
    public class MockBankingService : IBankingService
    {
        public async Task<TransactionResponse> MakeTransactionAsync(TransactionRequest transactionRequest)
        {
            return new() {Success = true, TransactionReference = Guid.NewGuid().ToString(), StatusCode = "1000"};
        }
    }
}
