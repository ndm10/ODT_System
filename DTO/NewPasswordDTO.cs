using ODT_System.Validation;
using System.ComponentModel.DataAnnotations;

namespace ODT_System.DTO
{
    public class NewPasswordDTO
    {
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [Required(ErrorMessage = "Vui lòng nhập email")]
        public string Email { get; set; } = null!;

        [PasswordValidation(ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự, ít nhất 1 ký tự đặc biệt, ít nhất 1 số và không có khoảng trắng!")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới")]
        public string OTP { get; set; } = null!;
    }
}
