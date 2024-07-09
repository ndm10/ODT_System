using ODT_System.Models;

namespace ODT_System.DTO
{
    public class FeedbackCommonDTO
    {
        public int Id { get; set; }

        public string Content { get; set; } = null!;

        public DateTime CreateAt { get; set; }

        public string Status { get; set; } = null!;

        public int CreateById { get; set; }

        public virtual UserCommonDTO CreateBy { get; set; } = null!;
    }
}
