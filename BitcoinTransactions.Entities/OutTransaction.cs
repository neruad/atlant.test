using System;

namespace BitcoinTransactions.Entities
{
    public class OutTransaction
    {
        public int Id { get; set; }

        public int WalletId { get; set; }

        public string Address { get; set; }

        public decimal Amount { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
