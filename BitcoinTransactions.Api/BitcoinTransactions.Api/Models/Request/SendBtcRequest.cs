using System.ComponentModel.DataAnnotations;

namespace BitcoinTransactions.Api.Models.Request
{
    /// <summary>
    /// Перевод BTC 
    /// </summary>
    public class SendBtcRequest
    {
        /// <summary>
        /// адрес кошелька, куда нужно отправить BTC
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Укажите адрес кошелька")]
        public string Address { get; set; }
        /// <summary>
        /// сумма перевода в BTC
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Неверная сумма перевода")]
        public decimal Amount { get; set; }
    }
}