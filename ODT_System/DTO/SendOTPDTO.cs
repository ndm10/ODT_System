using System.ComponentModel.DataAnnotations;

namespace ODT_System.DTO
{
    public class SendOTPDTO
    {
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [Required(ErrorMessage = "Vui lòng nhập email")]
        public string Email { get; set; } = null!;
    }
}
