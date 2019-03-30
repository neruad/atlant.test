using BitcoinTransactions.Entities;
using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BitcoinsTransaction.DataAccess.Repository
{
    public class TransactionRepository : IDisposable
    {
        private TransactionContext txContext = new TransactionContext();

        public async Task<InTransaction[]> GetLastInTxAsync()
        {
            using (txContext)
            using (var tran = txContext.Database.BeginTransaction())
            {
                try
                {
                    var result = await txContext.InTransactions.Where(x => x.Confirmations < 3 && !x.IsShown).ToArrayAsync();

                    foreach (var item in result)
                    {
                        item.IsShown = true;
                    }

                    await txContext.SaveChangesAsync();
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
            using (txContext)
            using (var tran = txContext.Database.BeginTransaction())
            {
                try
                {
                    var outTx = new OutTransaction
                    {
                        Address = address,
                        Amount = amount,
                        TimeStamp = DateTime.Now,
                        WalletId = 1
                    };
                    txContext.OutTransactions.Add(outTx);
                    await txContext.SaveChangesAsync();
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
                if (txContext != null)
                {
                    txContext.Dispose();
                    txContext = null;
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
