using BitcoindClient.Entities;
using BitcoindClient.Models;

namespace BitcoindClient
{
    public static class BitcoindCleintExt
    {
        public static Wallet[] GetWallets(this BitcoindClient client)
        {
            return client.MakeRequest<JsonRpcResponse<Wallet[]>>(Methods.backupwallet).Result;
        }
    }
}
