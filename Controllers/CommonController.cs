using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ODT_System.Services;
using ODT_System.Services.Interface;

namespace ODT_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly ICommonService _commonService;

        public CommonController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        [HttpGet("post")]
        public IActionResult GetAllPosts(int? pageIndex, int? pageSize, string? textSearch)
        {
            var posts = _commonService.GetPosts(pageIndex, pageSize, textSearch);
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
    }
}
