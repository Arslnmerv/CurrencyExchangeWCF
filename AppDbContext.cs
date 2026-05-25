using Microsoft.EntityFrameworkCore;

namespace CurrencyExchangeWCF
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserAccount> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=exchange.db");
        }
    }
}