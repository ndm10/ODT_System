using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ODT_System.DTO;
using ODT_System.Services.Interface;

namespace ODT_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("forgot-password/verify-email")]
        public IActionResult VerifyEmail([FromBody] VerifyEmailDTO verifyEmailDTO)
        {
            bool isExist = _accountService.VerifyEmail(verifyEmailDTO, out string message);

            if (!isExist)
            {
                return NotFound(message);
            }
            return Ok(message);
        }

        [HttpPost("forgot-password/send-otp")]
        public IActionResult SendOTP([FromBody] SendOTPDTO sendOTPDTO)
        {
            bool isSent = _accountService.SendOTP(sendOTPDTO, out string message);

            if (!isSent)
            {
                return BadRequest(message);
            }

            return Ok(message);
        }
    }
}
