using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using ODT_System.DTO;
using ODT_System.Services.Interface;
using System.Security.Claims;
using System.Text.Json;

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
            var user = _accountService.FindUserProfile(email);

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

        [Authorize]
        [HttpPut("change-password")]
        public IActionResult ChangePassword(ChangePasswordDTO changePasswordDTO)
        {
            // Get email from token
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (email == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error with authencation");
            }

            // Change password
            bool isChanged = _accountService.ChangePassword(changePasswordDTO, email, out string message);

            if (!isChanged)
            {
                return BadRequest(message);
            }

            return Ok(message);
        }

        [Authorize(Roles = "Tutor")]
        [HttpPost("post")]
        public IActionResult CreatePost(PostCreateDTO postCreateDTO)
        {
            // Get user id from token
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Xảy ra lỗi trong quá trình xác thực tài khoản!");
            }

            // Create post
            bool isCreated = _accountService.CreatePost(postCreateDTO, userEmail, out string message);
            if (!isCreated)
            {
                return BadRequest(message);
            }

            return Ok(message);
        }

        [Authorize(Roles = "Tutor")]
        [HttpGet("post")]
        public IActionResult ListPost(int? pageIndex, int? pageSize, string? status, string? textSearch)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Xảy ra lỗi trong quá trình xác thực tài khoản!");
            }

            var posts = _accountService.ListPost(userEmail, pageIndex, pageSize, status, textSearch);

            return Ok(posts);
        }

        [Authorize(Roles = "Tutor")]
        [HttpGet("post/{id}")]
        public IActionResult PostDetails(int id)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Xảy ra lỗi trong quá trình xác thực tài khoản!");
            }

            var post = _accountService.GetPostById(id, userEmail);

            if (post == null)
            {
                return BadRequest("Không tìm thấy bài viết hoặc bạn không có quyền quản lý bài viết này");
            }

            return Ok(post);
        }

        [Authorize(Roles = "Tutor")]
        [HttpPut("post")]
        public IActionResult UpdatePost(PostUpdateDTO postUpdateDTO)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Xảy ra lỗi trong quá trình xác thực tài khoản!");
            }

            bool isUpdated = _accountService.UpdatePost(postUpdateDTO, userEmail, out string message);

            if (!isUpdated)
            {
                return BadRequest(message);
            }

            return Ok(message);
        }

        [Authorize(Roles = "Tutor")]
        [HttpDelete("post/{id}")]
        public IActionResult DeletePost(int id)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Xảy ra lỗi trong quá trình xác thực tài khoản!");
            }

            bool isDeleted = _accountService.DeletePost(id, userEmail, out string message);

            if (!isDeleted)
            {
                return BadRequest(message);
            }

            return Ok(message);
        }

        [Authorize]
        [HttpGet("chat")]
        public IActionResult ListChat()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Xảy ra lỗi trong quá trình xác thực tài khoản!");
            }

            var chats = _accountService.ListChat(userEmail);

            return Ok(chats);
        }

        [Authorize]
        [HttpGet("chat/inbox/{withUser}")]
        public IActionResult GetInbox(int withUser)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Xảy ra lỗi trong quá trình xác thực tài khoản!");
            }

            var chats = _accountService.ListInbox(userEmail, withUser);

            return Ok(chats);
        }

        [Authorize]
        [HttpPost("chat")]
        public IActionResult Chat(ChatInBoxDTO chatInBoxDTO)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Xảy ra lỗi trong quá trình xác thực tài khoản!");
            }

            var isInboxSuccess = _accountService.TryInbox(chatInBoxDTO, userEmail, out string message);

            if (!isInboxSuccess)
            {
                return BadRequest(message);
            }

            return Ok(message);
        }
    }
}
