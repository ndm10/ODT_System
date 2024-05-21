using ODT_System.DTO;
using ODT_System.Models;
using ODT_System.Repository.Interface;
using ODT_System.Services.Interface;
using ODT_System.Utils.Interface;

namespace ODT_System.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMailHandler _mailHandler;
        private readonly IBcryptHandler _bcryptHandler;

        public AccountService(IUserRepository userRepository, IMailHandler mailHandler, IBcryptHandler bcryptHandler)
        {
            _userRepository = userRepository;
            _mailHandler = mailHandler;
            _bcryptHandler = bcryptHandler;
        }

        public bool VerifyEmail(VerifyEmailDTO verifyEmailDTO, out string message)
        {
            var email = verifyEmailDTO.Email;

            // Find user by email
            var user = _userRepository.FindByEmail(email);

            // Check if user is not found
            if (user == null)
            {
                message = "Email không tồn tại";
                return false;
            }

            message = "OTP đã được gửi";
            return true;

        }

        public bool SendOTP(SendOTPDTO sendOTPDTO, out string message)
        {

            var email = sendOTPDTO.Email;

            // Find user by email
            var user = _userRepository.FindByEmail(email);

            // Check if user is not found
            if (user == null)
            {
                message = "Email không tồn tại";
                return false;
            }

            //Generate OTP by random number contains 6 digits
            Random random = new Random();
            int otp = random.Next(100000, 999999);

            string subject = "Mã OTP để lấy lại mật khẩu";
            string body = "Mã OTP để lấy lại mật khẩu của bạn là: <b>" + otp + "</b>"
                + $"<br>Mã OTP có hiệu lực trong vòng 1 phút!"
                + $"<br>Vui lòng nhập mã OTP này để lấy lại mật khẩu!"
                + $"<br><strong>Vui lòng không tắt ứng dụng trong lúc lấy lại mật khẩu!<strong>"
                + $"<br><br>Trân trọng<br>";

            //Send OTP to email
            _mailHandler.Send(email,
                    subject,
                    body);

            string otpHash = _bcryptHandler.HashPassword(otp.ToString());
            message = otpHash;
            return true;
        }

        public bool VerifyOTP(VerifyOTPDTO verifyOTPDTO, out string message)
        {
            // Veriry OTP
            var isValid = _bcryptHandler.VerifyPassword(verifyOTPDTO.OTPEnter, verifyOTPDTO.OTPHash);

            if (!isValid)
            {
                message = "Mã OTP không đúng";
                return false;
            }

            message = "Mã OTP đúng";
            return true;
        }

        public bool NewPassword(NewPasswordDTO newPasswordDTO, out string message)
        {
            // Find user by email
            var user = _userRepository.FindByEmail(newPasswordDTO.Email);

            if (user == null)
            {
                message = "Email không tồn tại";
                return false;
            }

            // Hash new password
            user.Password = _bcryptHandler.HashPassword(newPasswordDTO.NewPassword);
            
            // Update new password
            _userRepository.Update(user);
            _userRepository.Save();

            message = "Đổi mật khẩu thành công";
            return true;
        }

        public User? FindByEmail(string email)
        {
            return _userRepository.FindByEmail(email);
        }

        public bool UpdateProfile(UpdateProfileDTO updateProfileDTO, string emailAccount)
        {
            // Find user by email
            var user = _userRepository.FindByEmail(emailAccount);

            if (user == null)
            {
                return false;
            }

            user.FullName = updateProfileDTO.FullName == null? user.FullName : updateProfileDTO.FullName;
            user.Phone = updateProfileDTO.Phone == null? user.Phone : updateProfileDTO.Phone;
            user.Gender = updateProfileDTO.Gender == null? user.Gender : updateProfileDTO.Gender.Value;
            user.Dob = updateProfileDTO.Dob == null? user.Dob : updateProfileDTO.Dob.Value;
            user.Desciption = updateProfileDTO.Desciption == null? user.Desciption : updateProfileDTO.Desciption;

            _userRepository.Update(user);
            _userRepository.Save();
            return true;
        }
    }
}
