using AutoMapper;
using ODT_System.DTO;
using ODT_System.Models;
using ODT_System.Repository.Interface;
using ODT_System.Services.Interface;
using ODT_System.Utils;
using ODT_System.Utils.Interface;

namespace ODT_System.Services
{
    public class AuthenticationService : IAuthenticationService
    {

        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IJWTHandler _jwtHandler;
        private readonly IBcryptHandler _bcryptHandler;

        public AuthenticationService(IMapper mapper, IUserRepository userRepository, IJWTHandler jWTHandler, IBcryptHandler bcryptHandler)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _jwtHandler = jWTHandler;
            _bcryptHandler = bcryptHandler;
        }

        public bool Login(UserLoginDTO userLoginDTO, out Dictionary<string, object> data)
        {
            //Map UserLoginDTO to User
            User userLogin = _mapper.Map<User>(userLoginDTO);

            //Check user is exist or not
            var userStoreDb = _userRepository.FindByEmailIncludeRole(userLogin.Email);
            if (userStoreDb == null)
            {
                data = null;
                return false;
            }

            // Validate password
            if (!_bcryptHandler.VerifyPassword(userLogin.Password, userStoreDb.Password))
            {
                data = null;
                return false;
            }

            //Generate token
            var tokenGenerate = _jwtHandler.GenerateToken(userStoreDb).ToString();

            string token = tokenGenerate == null ? "Error while generate token" : tokenGenerate;

            data = new Dictionary<string, object>
            {
                { "token", token },
                { "userId", userStoreDb.Id },
                { "fullName", userStoreDb.FullName},
                { "roleId", userStoreDb.Role.Id},
                {"expireTime", DateTime.Now.AddDays(30).ToString() }
            };

            return true;
        }

        public bool Register(UserRegisterDTO user, out string message)
        {
            // Check email is exist or not
            var userExist = _userRepository.FindByEmail(user.Email);
            if (userExist != null)
            {
                message = "Email đã tồn tại";
                return false;
            }

            // If email not exist, create new user
            User newUser = _mapper.Map<User>(user);

            // Hash password
            newUser.Password = _bcryptHandler.HashPassword(newUser.Password);

            // Add new user to database
            _userRepository.Create(newUser);
            _userRepository.Save();

            message = "Tạo tài khoản thành công";
            return true;
        }
    }
}
