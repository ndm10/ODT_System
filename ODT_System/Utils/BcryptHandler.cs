using ODT_System.Utils.Interface;

namespace ODT_System.Utils
{
    public class BcryptHandler : IBcryptHandler
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string userPass, string accountPass)
        {
            return BCrypt.Net.BCrypt.Verify("12345678h@", "$2a$12$HACLPFGfdrhNHIVYL3368Of3wnpYN41QK/SkH6FOhiUec3ojWz4Bq");
        }
    }
}
