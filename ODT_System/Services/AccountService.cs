using ODT_System.DTO;
using ODT_System.Repository.Interface;
using ODT_System.Services.Interface;
using ODT_System.Utils.Interface;

namespace ODT_System.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMailHandler _mailHandler;


        public AccountService(IUserRepository userRepository, IMailHandler mailHandler)
        {
            _userRepository = userRepository;
            _mailHandler = mailHandler;
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

            message = "Mã OTP đã được gửi tới email của bạn!";
            return true;
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
    }
}
