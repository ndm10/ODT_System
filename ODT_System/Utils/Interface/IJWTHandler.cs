using ODT_System.Models;

namespace ODT_System.Utils.Interface
{
    public interface IJWTHandler
    {
        public object GenerateToken(User user);
    }
}
