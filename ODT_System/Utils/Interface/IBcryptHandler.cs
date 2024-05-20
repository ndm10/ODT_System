namespace ODT_System.Utils.Interface
{
    public interface IBcryptHandler
    {
        public string HashPassword(string password);
        public bool VerifyPassword(string text, string hash);
    }
}
