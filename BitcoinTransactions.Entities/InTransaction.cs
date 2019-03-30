using System;

namespace BitcoinTransactions.Entities
{
    public class InTransaction
    {
        public int Id { get; set; }

        public int Confirmations { get; set; }

        public DateTime TimeStamp { get; set; }

        public decimal Amount { get; set; }

        public string Address { get; set; }

        public bool IsShown { get; set; }
    }
}
