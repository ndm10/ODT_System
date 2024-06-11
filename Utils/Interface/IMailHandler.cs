namespace ODT_System.Utils.Interface
{
    public interface IMailHandler
    {
        public void Send(string recipient, string subject, string body);
    }
}
