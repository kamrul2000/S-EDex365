using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;

namespace S_EDex365.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherWalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        public TeacherWalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet("s/GetTotalTeacherBalance")]
        public async Task<ActionResult<TeacherWallet>> GetTotalTeacherBalance(Guid userId)
        {
            var wallet = await _walletService.GetTeacherWalletAsync(userId);

            if (wallet == null)
                return NotFound("Wallet not found for the user.");

            return Ok("Balance:" + wallet.Balance); // or wallet.Amount, depending on your model
        }
    }
}
