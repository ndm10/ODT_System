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

        public bool IsValidLogin(UserLoginDTO userLoginDTO, out string token)
        {
            //Map UserLoginDTO to User
            User user = _mapper.Map<User>(userLoginDTO);

            //Check user is exist or not
            var userLogin = _userRepository.FindByEmail(user.Email);
            if (userLogin == null)
            {
                token = string.Empty;
                return false;
            }

            // Validate password
            if (!_bcryptHandler.VerifyPassword(userLogin.Password, user.Password))
            {
                token = string.Empty;
                return false;
            }

            //Generate token
            var tokenGenerate = _jwtHandler.GenerateToken(userLogin).ToString();

            token = tokenGenerate == null ? "Error while generate token" : tokenGenerate;
            return true;
        }
    }
}
