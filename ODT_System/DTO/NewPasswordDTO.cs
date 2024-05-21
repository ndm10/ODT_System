using System.ComponentModel.DataAnnotations;

namespace ODT_System.DTO
{
    public class NewPasswordDTO
    {
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [Required(ErrorMessage = "Vui lòng nhập email")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới")]
        public string NewPassword { get; set; } = null!;
    }
}
