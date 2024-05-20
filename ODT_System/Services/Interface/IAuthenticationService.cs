using ODT_System.DTO;

namespace ODT_System.Services.Interface
{
    public interface IAuthenticationService
    {
        public bool IsValidLogin(UserLoginDTO user, out string token);
    }
}
