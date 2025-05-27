using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using S_EDex365.API.Models.Bkash;
using S_EDex365.API.Services.Bkash;

namespace S_EDex365.API.Controllers.Bkash
{
    [Route("api/[controller]")]
    [ApiController]
    public class BkashController : ControllerBase
    {
        private readonly BkashService _bkash;

        public BkashController(BkashService bkash) => _bkash = bkash;

        [HttpPost("create")]
        public async Task<IActionResult> CreatePayment([FromBody] BkashCreatePaymentRequest request)
        {
            var token = await _bkash.GetTokenAsync();
            var response = await _bkash.CreatePaymentAsync(token, request);
            return Ok(response);
        }

        [HttpPost("execute/{paymentId}")]
        public async Task<IActionResult> ExecutePayment(string paymentId)
        {
            var token = await _bkash.GetTokenAsync();
            var response = await _bkash.ExecutePaymentAsync(token, paymentId);
            return Ok(response);
        }
    }
}
