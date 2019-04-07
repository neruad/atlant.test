using BitcoinTransactions.DataAccess.Repository;
using BitcoinTransactions.Api.Models;
using BitcoinTransactions.Api.Models.Request;
using BitcoinTransactions.Api.Models.Response;
using BitcoinTransactions.Api.Models.ViewModel;
using BitcoinTransactions.Entities;
using Microsoft.Web.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Security.AntiXss;

namespace BitcoinTransactions.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [RoutePrefix("v{version:apiVersion}/transaction")]
    public class TransactionController : ApiController
    {
        private TransactionRepository _txRepository;

        public TransactionController()
        {
            //хорошей практикой будет использовать DI
            //Но по ТЗ нельзя использовать сторонние решения
            _txRepository = new TransactionRepository();
        }
        /// <summary>
        /// отправляет BTC с одного из hot кошельков на адрес пользователя
        /// </summary>
        /// <param name="request">address ­- адрес кошелька, куда нужно отправить BTC. amount -­ сумма перевода в BTC</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("sendBtc")]
        public async Task<ApiResponse> SendBtc([FromBody] SendBtcRequest request)
        {
            var addr = AntiXssEncoder.HtmlEncode(request.Address, true);
            var result = await _txRepository.SaveOutTxAsync(addr, request.Amount);

            return new ApiResponse { Code = result.GetHashCode(), Message = result.ToString() };
        }

        /// <summary>
        /// информация о последних поступлениях на кошельки
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("getLast")]
        public async Task<ApiResponse<InTx[]>> GetLast()
        {
            var transactions = await _txRepository.GetLastInTxAsync();
            return new ApiResponse<InTx[]>
            {
                Code = ResultCode.Ok.GetHashCode(),
                Data = transactions.MapToInTx()
            };
        }
    }
}
