using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ODT_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet("post")]
        public IActionResult GetAllPost()
        {
            return Ok("Admin");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("post/appove")]
        public IActionResult AppovePost()
        {
            return Ok("Admin");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("post/{id}")]
        public IActionResult PostDetails(int id)
        {
            return Ok("Admin");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("user")]
        public IActionResult GetAllUser()
        {
            return Ok("Admin");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("user/{id}")]
        public IActionResult GetUserDetails(int id)
        {
            return Ok("Admin");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("user")]
        public IActionResult UpdateUserStatus()
        {
            return Ok("Admin");
        }
    }
}
