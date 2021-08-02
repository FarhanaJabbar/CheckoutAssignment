using System;
using System.Threading.Tasks;
using CheckoutApi.Database;
using CheckoutApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CheckoutApi.Services
{
    public class TransactionRepository :ITransactionRepository
    {
        private readonly CheckoutDbContext _checkoutDbContext;
        public TransactionRepository(CheckoutDbContext checkoutDbContext)
        {
            _checkoutDbContext = checkoutDbContext;
        }
        public async Task AddTransactionAsync(Transaction transaction)
        {
            await _checkoutDbContext.Transactions.AddAsync(transaction);
            await _checkoutDbContext.SaveChangesAsync();
        }

        public async Task<Transaction> GetTransactionAsync(string transactionReference)
        {
            return await _checkoutDbContext.Transactions.FirstOrDefaultAsync(x=> x.Reference == transactionReference);
        }

        public async Task DeleteTransactionAsync(Guid transactionId)
        {
            var transaction = await _checkoutDbContext.Transactions.FindAsync(transactionId);
            if (transaction != null)
            {
                _checkoutDbContext.Transactions.Remove(transaction);
                await _checkoutDbContext.SaveChangesAsync();
            }
        }
    }
}
