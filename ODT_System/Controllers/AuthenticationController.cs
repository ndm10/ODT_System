using Microsoft.AspNetCore.Mvc;
using ODT_System.DTO;
using ODT_System.Models;
using ODT_System.Services.Interface;

namespace ODT_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : BaseController
    {

        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDTO userLoginDTO)
        {
            //validate input
            var isValid = IsValidate(out var validationErrors);
            if (!isValid)
            {
                return BadRequest(new { message = "Dữ liệu đầu vào không hợp lệ", errors = validationErrors });
            }

            // Login user
            bool isValidLogin = _authenticationService.Login(userLoginDTO, out string token);

            // Return error if login fail
            if (!isValidLogin)
            {
                return Unauthorized(new { message = "Sai email hoặc mật khẩu" });
            }

            return Ok(token);
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterDTO user)
        {
            //validate input
            var isValid = IsValidate(out var validationErrors);
            if (!isValid)
            {
                return BadRequest(new { message = "Dữ liệu đầu vào không hợp lệ", errors = validationErrors });
            }

            // Register user
            bool isRegister = _authenticationService.Register(user, out string message);

            // Return error if register fail
            if (!isRegister)
            {
                return BadRequest(message);
            }

            return Ok(message);
        }
    }
}
