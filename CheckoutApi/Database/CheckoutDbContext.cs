using CheckoutApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CheckoutApi.Database
{
    public class CheckoutDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public CheckoutDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public virtual DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("CheckoutDb");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
