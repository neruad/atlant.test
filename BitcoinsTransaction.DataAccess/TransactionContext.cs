using BitcoinTransactions.Entities;
using System.Data.Entity;

namespace BitcoinsTransaction.DataAccess
{
    class TransactionContext : DbContext
    {
        public TransactionContext() : base("DBConnection")
        {
        }

        public DbSet<OutTransaction> OutTransactions { get; set; }
        public DbSet<InTransaction> InTransactions { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
     }
}
