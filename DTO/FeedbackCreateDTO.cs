using System.ComponentModel.DataAnnotations;

namespace ODT_System.DTO
{
    public class FeedbackCreateDTO
    {
        [Required(ErrorMessage = "Vui lòng nhập feed back")]
        public string Content { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập người nhận feed back")]
        public int FeedbeckToId { get; set; }
    }
}
