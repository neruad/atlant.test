using BitcoinTransactions.Entities;
using System.Data.Entity;

namespace BitcoinTransactions.DataAccess
{
    class BitcoinContext : DbContext
    {
        public BitcoinContext() : base("DBConnection")
        {
        }

        public DbSet<OutTransaction> OutTransactions { get; set; }
        public DbSet<InTransaction> InTransactions { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
     }
}
