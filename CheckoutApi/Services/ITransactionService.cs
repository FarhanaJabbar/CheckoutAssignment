using System.Threading.Tasks;
using CheckoutApi.Models;

namespace CheckoutApi.Services
{
    public interface ITransactionService
    {
        Task<TransactionResponse> MakePaymentAsync(TransactionRequest transactionRequest);
        Task<TransactionDetailResponse> GeTransactionDetailAsync(string transactionReference);
    }
}