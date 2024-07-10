using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ODT_System.Services;
using ODT_System.Services.Interface;
using System.Security.Claims;

namespace ODT_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly ICommonService _commonService;
        private readonly IFeedbackService _feedbackService;

        public CommonController(ICommonService commonService, IFeedbackService feedbackService)
        {
            _commonService = commonService;
            _feedbackService = feedbackService;
        }

        [HttpGet("post")]
        public IActionResult GetAllPosts(int? pageIndex, int? pageSize, string? textSearch, string? addressSearch)
        {
            var posts = _commonService.GetPosts(pageIndex, pageSize, textSearch, addressSearch);
            return Ok(posts);
        }

        [HttpGet("post/{id}")]
        public IActionResult GetPostById(int id)
        {
            var post = _commonService.GetPostById(id);
            if (post == null)
            {
                return NotFound("Không tìm thấy bài viết này");
            }
            return Ok(post);
        }

        [HttpGet("tutors")]
        public IActionResult GetAllTutors(int? pageIndex, int? pageSize, string? textSearch)
        {
            var users = _commonService.GetTutors(pageIndex, pageSize, textSearch);
            return Ok(users);
        }

        [HttpGet("tutor-details/{id}")]
        public IActionResult ViewProfile(int id)
        {

            // Get user by email
            var user = _commonService.FindUserProfile(id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);

        }

        [HttpGet("get-feedbacks-by-id/{id}")]
        public IActionResult GetFeedbacksById(int id)
        {
            var feedbacks = _feedbackService.GetFeedbacksById(id);

            return Ok(feedbacks);
        }
    }
}
