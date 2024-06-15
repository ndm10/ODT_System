using ODT_System.Models;

namespace ODT_System.DTO
{
    public class ChatDTO
    {
        public int Id { get; set; }

        public int From { get; set; }

        public int To { get; set; }

        public string Content { get; set; } = null!;

        public DateTime Time { get; set; }

        public virtual UserChatDTO FromNavigation { get; set; } = null!;

        public virtual UserChatDTO ToNavigation { get; set; } = null!;
    }
}
