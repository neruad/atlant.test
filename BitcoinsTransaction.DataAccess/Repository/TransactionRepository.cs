using BitcoinTransactions.Entities;
using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BitcoinTransactions.DataAccess.Repository
{
    public class TransactionRepository : IDisposable
    {
        private BitcoinContext dbContext = new BitcoinContext();

        public async Task<InTransaction[]> GetLastInTxAsync()
        {
            using (dbContext)
            using (var tran = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var result = await dbContext.InTransactions.Where(x => x.Confirmations < 3 && !x.IsShown).ToArrayAsync();

                    foreach (var item in result)
                    {
                        item.IsShown = true;
                    }

                    await dbContext.SaveChangesAsync();
                    tran.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    tran?.Rollback();
                    return null;
                }
            }
        }

        public async Task<ResultCode> SaveOutTxAsync(string address, decimal amount)
        {
            using (dbContext)
            using (var tran = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var walletRepo = new WalletRepository();
                    var wallet = await walletRepo.FindByAddressAsync(address);

                    var outTx = new OutTransaction
                    {
                        Address = address,
                        Amount = amount,
                        TimeStamp = DateTime.Now,
                        WalletId = wallet?.Id ?? 0
                    };
                    dbContext.OutTransactions.Add(outTx);
                    await dbContext.SaveChangesAsync();
                    tran.Commit();
                    return ResultCode.Ok;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    tran?.Rollback();
                    return ResultCode.Error;
                }
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
