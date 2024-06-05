using ODT_System.DTO;
using ODT_System.Models;

namespace ODT_System.Services.Interface
{
    public interface IAccountService
    {
        bool ChangePassword(ChangePasswordDTO changePasswordDTO, string email, out string message);
        public ViewProfileDTO? FindUserProfile(string email);
        public bool NewPassword(NewPasswordDTO newPasswordDTO, out string message);
        public bool UpdateProfile(UpdateProfileDTO updateProfileDTO, string emailAccount);
        public bool VerifyEmail(VerifyEmailDTO verifyEmailDTO, out string message);
    }
}
