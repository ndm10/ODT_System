using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ODT_System.DTO;
using ODT_System.Services.Interface;

namespace ODT_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        /// <summary>
        /// Get all post to manage (Admin)
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("post")]
        public IActionResult GetAllPost(int? pageIndex, int? pageSize, string? status, string? textSearch)
        {
            var posts = _adminService.GetAllPost(pageIndex, pageSize, status, textSearch);
            return Ok(posts);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("post/manage")]
        public IActionResult AppovePost(PostApproveDTO postApproveDTO)
        {
            var isAppoved = _adminService.AppovePost(postApproveDTO, out string message);

            if (!isAppoved)
            {
                return BadRequest(message);
            }

            return Ok(message);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("post/{id}")]
        public IActionResult PostDetails(int id)
        {
            var post = _adminService.GetPostDetails(id);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("user")]
        public IActionResult GetAllUser(int? pageIndex, int? pageSize, bool? status, string? textSearch)
        {
            var users = _adminService.GetAllUser(pageIndex, pageSize, status, textSearch);
            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("user/{id}")]
        public IActionResult GetUserDetails(int id)
        {
            var user = _adminService.GetUserDetails(id);
                
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("user")]
        public IActionResult UpdateUserStatus(UserUpdateAdminDTO userUpdateAdminDTO)
        {
            var isUpdated = _adminService.UpdateUserStatus(userUpdateAdminDTO, out string message);

            if (!isUpdated)
            {
                return BadRequest(message);
            }

            return Ok(message);
        }
    }
}
