using ODT_System.Validation;
using System.ComponentModel.DataAnnotations;

namespace ODT_System.DTO
{
    public class UserRegisterDTO
    {
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [Required(ErrorMessage = "Vui lòng nhập email")]
        public string Email { get; set; } = null!;

        [PasswordValidation(ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự, ít nhất 1 ký tự đặc biệt, ít nhất 1 số và không có khoảng trắng!")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng chọn giới tính")]
        public bool Gender { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ngày sinh")]
        public DateOnly Dob { get; set; }

        [RegularExpression(@"^0[0-9]{9}$", ErrorMessage = "Vui lòng nhập đúng số điện thoại định dạng Việt Nam!")]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng chọn vai trò")]
        public int RoleId { get; set; }
    }
}
