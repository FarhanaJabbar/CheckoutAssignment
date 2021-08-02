using System.Threading.Tasks;
using CheckoutApi.Models;

namespace CheckoutApi.Services
{
    public interface IBankingService
    {
        Task<TransactionResponse> MakeTransactionAsync(TransactionRequest transactionRequest);
    }
}