using BitcoindClient;
using BitcoindClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitcoindClientTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new BitcoindClient.BitcoindClient();
            var request = new JsonRpcRequest (1, Methods.dumpwallet.ToString(), null);

            var response = client.MakeRequest<JsonRpcResponse<string>>(Methods.dumpwallet, null);
        }
    }
}
