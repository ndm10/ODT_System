using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ODT_System.DTO;
using ODT_System.Repository.Interface;
using ODT_System.Services.Interface;
using System.Security.Claims;

namespace ODT_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("forgot-password/verify-email")]
        public IActionResult VerifyEmail([FromBody] VerifyEmailDTO verifyEmailDTO)
        {
            //validate input
            var isValid = IsValidate(out var validationErrors);
            if (!isValid)
            {
                return BadRequest(new { message = "Dữ liệu đầu vào không hợp lệ", errors = validationErrors });
            }

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
            //validate input
            var isValid = IsValidate(out var validationErrors);
            if (!isValid)
            {
                return BadRequest(new { message = "Dữ liệu đầu vào không hợp lệ", errors = validationErrors });
            }

            bool isSent = _accountService.SendOTP(sendOTPDTO, out string message);

            if (!isSent)
            {
                return BadRequest(message);
            }

            return Ok(message);
        }

        [HttpPost("forgot-password/verify-otp")]
        public IActionResult VerifyOTP([FromBody] VerifyOTPDTO verifyOTPDTO)
        {
            //validate input
            var isValid = IsValidate(out var validationErrors);
            if (!isValid)
            {
                return BadRequest(new { message = "Dữ liệu đầu vào không hợp lệ", errors = validationErrors });
            }

            bool isVerified = _accountService.VerifyOTP(verifyOTPDTO, out string message);

            if (!isVerified)
            {
                return BadRequest(message);
            }

            return Ok(message);
        }

        [HttpPut("forgot-password/new-password")]
        public IActionResult NewPassword([FromBody] NewPasswordDTO newPasswordDTO)
        {
            //validate input
            var isValid = IsValidate(out var validationErrors);
            if (!isValid)
            {
                return BadRequest(new { message = "Dữ liệu đầu vào không hợp lệ", errors = validationErrors });
            }

            bool isChanged = _accountService.NewPassword(newPasswordDTO, out string message);

            if (!isChanged)
            {
                return BadRequest(message);
            }

            return Ok(message);
        }

        [Authorize]
        [HttpGet("profile/view")]
        public IActionResult ViewProfile()
        {
            // Get email from token
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Có lỗi xảy ra khi xác thực thông tin người dùng");
            }

            // Get user by email
            var user = _accountService.FindByEmail(email);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);

        }

        [HttpPut("profile/update")]
        public IActionResult UpdateProfile(UpdateProfileDTO updateProfileDTO)
        {
            // Get email from token
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error with authencation");
            }

            // update user by email
            var isUPdated = _accountService.UpdateProfile(updateProfileDTO, email);

            if (!isUPdated)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Có lỗi xảy ra khi xác thực thông tin người dùng");
            }

            return Ok("Cập nhật thông tin thành công");
        }
    }
}
