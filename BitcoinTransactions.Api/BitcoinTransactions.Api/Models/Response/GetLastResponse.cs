using BitcoinTransactions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitcoinTransactions.Api.Models.Response
{
    public class GetLastResponse
    {
        public OutTransaction[] LastTr { get; set; }
    }
}