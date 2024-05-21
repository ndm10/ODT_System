using ODT_System.DTO;
using ODT_System.Models;

namespace ODT_System.Services.Interface
{
    public interface IAccountService
    {
        public User? FindByEmail(string email);
        public bool NewPassword(NewPasswordDTO newPasswordDTO, out string message);
        public bool SendOTP(SendOTPDTO sendOTPDTO, out string message);
        public bool UpdateProfile(UpdateProfileDTO updateProfileDTO, string emailAccount);
        public bool VerifyEmail(VerifyEmailDTO verifyEmailDTO, out string message);
        public bool VerifyOTP(VerifyOTPDTO verifyOTPDTO, out string message);
    }
}
