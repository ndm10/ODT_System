using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public AccountService(IUserRepository userRepository, IMailHandler mailHandler,
            IBcryptHandler bcryptHandler, IMapper mapper, IMemoryCache cache)
        {
            _userRepository = userRepository;
            _mailHandler = mailHandler;
            _bcryptHandler = bcryptHandler;
            _mapper = mapper;
            _cache = cache;
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

            //Generate OTP by random number contains 6 digits
            Random random = new Random();
            int otp = random.Next(100000, 999999);

            string subject = "Mã OTP để lấy lại mật khẩu";
            string body = "Mã OTP để lấy lại mật khẩu của bạn là: <b>" + otp + "</b>"
                + $"<br>Mã OTP có hiệu lực trong vòng 1 phút!"
                + $"<br>Vui lòng nhập mã OTP này để lấy lại mật khẩu!"
                + $"<br><strong>Vui lòng không tắt ứng dụng trong lúc lấy lại mật khẩu!<strong>"
                + $"<br><br>Trân trọng.<br>";

            //Send OTP to email
            _mailHandler.Send(email,
                    subject,
                    body);

            // Set time expire cache for OTP
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

            // Store OTP to cache
            _cache.Set(email, otp.ToString(), cacheEntryOptions);

            message = "OTP đã được gửi";
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

            var isOtpExist = _cache.TryGetValue(newPasswordDTO.Email, out string otp);

            // Check OTP
            if (!isOtpExist)
            {
                message = "OTP không còn tồn tại!";
                return false;
            }
            else if (otp != newPasswordDTO.OTP)
            {
                message = "OTP không chính xác!";
                return false;
            }

            // Hash new password
            user.Password = _bcryptHandler.HashPassword(newPasswordDTO.Password);

            // Update new password
            _userRepository.Update(user);
            _userRepository.Save();

            message = "Đổi mật khẩu thành công";
            return true;
        }

        public ViewProfileDTO? FindUserProfile(string email)
        {
            var user = _userRepository.FindByEmail(email);
            var profileDTO = _mapper.Map<ViewProfileDTO>(user);
            return profileDTO;
        }

        public bool UpdateProfile(UpdateProfileDTO updateProfileDTO, string emailAccount)
        {
            // Find user by email
            var user = _userRepository.FindByEmail(emailAccount);

            if (user == null)
            {
                return false;
            }

            user.FullName = updateProfileDTO.FullName == null ? user.FullName : updateProfileDTO.FullName;
            user.Phone = updateProfileDTO.Phone == null ? user.Phone : updateProfileDTO.Phone;
            user.Gender = updateProfileDTO.Gender == null ? user.Gender : updateProfileDTO.Gender.Value;
            user.Dob = updateProfileDTO.Dob == null ? user.Dob : updateProfileDTO.Dob.Value;
            user.Desciption = updateProfileDTO.Desciption == null ? user.Desciption : updateProfileDTO.Desciption;

            _userRepository.Update(user);
            _userRepository.Save();
            return true;
        }
    }
}
