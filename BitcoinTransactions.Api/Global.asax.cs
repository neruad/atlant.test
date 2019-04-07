using BitcoinTransactions.DataAccess.Repository;
using System;
using System.Diagnostics;
using System.Web.Http;

namespace BitcoinTransactions.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            SyncWallets();
        }

        protected void SyncWallets()
        {
            try
            {
                var walletRepo = new WalletRepository();
                walletRepo.UpdateWallets();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
