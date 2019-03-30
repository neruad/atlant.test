using System.Configuration;

namespace BitcoindClient
{
    public class BitcoindClient
    {
        private string  _url {get;set;}
        public BitcoindClient()
        {
            _url = ConfigurationManager.ConnectionStrings["BitcoindAddress"].ToString();
        }

        public BitcoindClient(string url)
        {
            _url = url;
        }


    }
}
