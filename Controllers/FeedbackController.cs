using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ODT_System.DTO;
using ODT_System.Services.Interface;
using System.Security.Claims;

namespace ODT_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : BaseController
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateFeedback([FromBody] FeedbackCreateDTO feedbackDTO)
        {
            // Get email from token
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (email == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error with authencation");
            }

            //validate input
            var isValid = IsValidate(out var validationErrors);
            if (!isValid)
            {
                return BadRequest(new { message = "Dữ liệu đầu vào không hợp lệ", errors = validationErrors });
            }

            var isCreated = _feedbackService.CreateFeedback(feedbackDTO, email, out string message);

            if (!isCreated)
            {
                return BadRequest(message);
            }

            return Ok(message);
        }
    }
}
