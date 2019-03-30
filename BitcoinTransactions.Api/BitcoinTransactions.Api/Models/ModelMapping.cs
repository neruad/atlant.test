using BitcoinTransactions.Api.Models.ViewModel;
using BitcoinTransactions.Entities;
using System.Linq;

namespace BitcoinTransactions.Api.Models
{
    /// <summary>
    /// Маппинг сущностей в модели апи
    /// Конечно, для этого есть уже готовые решения такие как AutoMapper и др
    /// Но по ТЗ нельзя использовать сторонние решения
    /// </summary>
    public static class ModelMapping
    {
        public static InTx[] MapToInTx(this InTransaction[] transactions)
        {
            return transactions?.Select(ToInTx).ToArray();
        }

        public static InTx ToInTx(this InTransaction transaction)
        {
            return transaction == null 
                ? null 
                : new InTx
                    {
                        Address = transaction.Address,
                        Amount = transaction.Amount,
                        Confirmations = transaction.Confirmations,
                        RegDate = transaction.TimeStamp
                    };
        }
    }
}