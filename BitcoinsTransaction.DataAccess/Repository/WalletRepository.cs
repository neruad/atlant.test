using BitcoindClient;
using BitcoinTransactions.Entities;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BitcoinTransactions.DataAccess.Repository
{
    public class WalletRepository : IDisposable
    {
        private BitcoinContext dbContext = new BitcoinContext();

        private Wallet ProduceWallet(BitcoindClient.Entities.Wallet wallet)
        {
            return new Wallet
            {
                Address = wallet.Address,
                Balance = wallet.Balance,
                Id = wallet.Id
            };
        }

        public void UpdateWallets()
        {
            var client = new BitcoindClient.BitcoindClient();
            var wallets = client.GetWallets();
            using (dbContext)
            using (var tran = dbContext.Database.BeginTransaction())
            {
                foreach (var wallet in wallets)
                {
                    SaveOrUpdate(ProduceWallet(wallet));
                }

                dbContext.SaveChanges();
            }
        }

        public async Task<Wallet> FindByAddressAsync(string address)
        {
            using (dbContext)
            using (var tran = dbContext.Database.BeginTransaction())
            {
                return await dbContext.Wallets.SingleOrDefaultAsync(x => x.Address == address);
            }
        }

        public async Task<Wallet> GetByIdAsync(int walletId)
        {
            using (dbContext)
            using (var tran = dbContext.Database.BeginTransaction())
            {
                return await dbContext.Wallets.SingleOrDefaultAsync(x => x.Id == walletId);
            }
        }

        public void SaveOrUpdate(Wallet wallet)
        {
            var oldWallet = dbContext.Wallets.FirstOrDefault(x => x.Id == wallet.Id);
            if (oldWallet != null)
            {
                oldWallet.Balance = wallet.Balance;
            }
            else
            {
                dbContext.Wallets.Add(wallet);
            }
        }
        
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (dbContext != null)
                {
                    dbContext.Dispose();
                    dbContext = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
