using Microsoft.AspNetCore.Mvc;
using ODT_System.DTO;
using ODT_System.Models;
using ODT_System.Services.Interface;

namespace ODT_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
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
                return BadRequest(new { message = "Validation errors", errors = validationErrors });
            }

            //Login user
            bool isValidLogin = _authenticationService.IsValidLogin(userLoginDTO, out string token);
            if (isValidLogin == false)
            {
                return Unauthorized(new { message = "Sai email hoặc mật khẩu" });
            }
            return Ok(token);
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            //validate input
            var isValid = IsValidate(out var validationErrors);
            if (!isValid)
            {
                return BadRequest(new { message = "Validation errors", errors = validationErrors });
            }

            //Register user
            return Ok();
        }

          protected bool IsValidate(out Dictionary<string, string> validationErrors)
        {
            validationErrors = new Dictionary<string, string>();

            //validate input
            if (!ModelState.IsValid)
            {

                //Get all errors
                foreach (var modelStateEntry in ModelState)
                {
                    var propertyName = modelStateEntry.Key;
                    var errorMessages = modelStateEntry.Value.Errors
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    //Add to validation errors
                    validationErrors.Add(propertyName.ToLower(), string.Join(", ", errorMessages));
                }

                // Return false if has validation errors
                return false;
            }

            // Return true if no validation errors
            return true;
        }
    }
}
