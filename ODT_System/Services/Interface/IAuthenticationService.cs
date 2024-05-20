using ODT_System.DTO;
using ODT_System.Models;

namespace ODT_System.Services.Interface
{
    public interface IAuthenticationService
    {
        public bool Login(UserLoginDTO user, out string token);
        public bool Register(UserRegisterDTO user, out string message);
    }
}
