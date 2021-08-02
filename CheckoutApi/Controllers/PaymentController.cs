using System.Threading.Tasks;
using CheckoutApi.Models;
using CheckoutApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CheckoutApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public PaymentController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost]
        public async Task<ActionResult<TransactionResponse>> MakePayment(TransactionRequest transactionRequest)
        {
            if (!ModelState.IsValid) return BadRequest();
            try
            {
                var result = await _transactionService.MakePaymentAsync(transactionRequest);
                return Ok(result);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error");
            }
        }

        [HttpGet]
        [Route("{paymentReference}")]
        public async Task<ActionResult<TransactionDetailResponse>> GetPaymentDetails(string paymentReference)
        {
            try
            {
                var result = await _transactionService.GeTransactionDetailAsync(paymentReference);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error");
            }
        }
    }
}
