using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;

namespace S_EDex365.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentWalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        public StudentWalletController(IWalletService walletService)
        {
            _walletService= walletService;
        }

        [HttpGet("s/GetTotalBalance")]
        public async Task<ActionResult<StudentWallet>> GetTotalBalance(Guid userId)
        {
            var wallet = await _walletService.GetStudentWalletAsync(userId);

            if (wallet == null)
                return NotFound("Wallet not found for the user.");

            return Ok("Balance:" + wallet.Balance); // or wallet.Amount, depending on your model
        }
        [HttpGet("s/StudentTransaction")]
        public async Task<ActionResult<StudentTransaction>> GetStudentTransaction(Guid userId)
        {
            var transInfo = await _walletService.GetAllTransactionByAsync(userId);

            if (transInfo == null)
                return Ok("No Transaction is here...");

            return Ok(transInfo);
        }
        [HttpGet("s/StudentAllCostTransaction")]
        public async Task<ActionResult<StudentCostTransaction>> GetStudentAllCostTransaction(Guid userId)
        {
            var transInfo = await _walletService.GetAllCostTransactionByAsync(userId);

            if (transInfo == null)
                return Ok("No Transaction is here...");

            return Ok(transInfo);
        }

    }
}
