using System;
using System.Threading.Tasks;
using CheckoutApi.Models;

namespace CheckoutApi.Services
{
    public interface ITransactionRepository
    {
        Task AddTransactionAsync(Transaction transactionDetail);
        Task<Transaction> GetTransactionAsync(string transactionId);
        Task DeleteTransactionAsync(Guid transactionId);
    }
}