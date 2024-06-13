using ODT_System.DTO;
using ODT_System.Models;

namespace ODT_System.Services.Interface
{
    public interface IAuthenticationService
    {
        public bool Login(UserLoginDTO user, out Dictionary<string, object> data, out string message);
        public bool Register(UserRegisterDTO user, out string message);
    }
}
