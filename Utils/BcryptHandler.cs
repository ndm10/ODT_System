using ODT_System.Utils.Interface;

namespace ODT_System.Utils
{
    public class BcryptHandler : IBcryptHandler
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string text, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(text, hash);
        }
    }
}
