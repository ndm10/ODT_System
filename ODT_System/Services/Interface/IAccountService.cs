using ODT_System.DTO;

namespace ODT_System.Services.Interface
{
    public interface IAccountService
    {
        bool SendOTP(SendOTPDTO sendOTPDTO, out string message);
        bool VerifyEmail(VerifyEmailDTO verifyEmailDTO, out string message);
    }
}
