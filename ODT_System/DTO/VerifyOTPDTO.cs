using System.ComponentModel.DataAnnotations;

namespace ODT_System.DTO
{
    public class VerifyOTPDTO
    {
        [Required(ErrorMessage = "Vui lòng nhập mã OTP")]
        public string OTPEnter { get; set; } = null!;
        [Required(ErrorMessage = "Vui lòng nhập mã OTP đã mã hóa")]
        public string OTPHash { get; set; } = null!;
    }
}
