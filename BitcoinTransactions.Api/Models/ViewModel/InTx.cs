using System;

namespace BitcoinTransactions.Api.Models.ViewModel
{
    public class InTx
    {
        public DateTime RegDate { get; set; }

        public string Address { get; set; }

        public decimal Amount { get; set; }

        public int Confirmations { get; set; }
    }
}