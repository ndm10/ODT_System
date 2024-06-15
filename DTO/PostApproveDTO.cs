using ODT_System.Enums;
using System.ComponentModel.DataAnnotations;

namespace ODT_System.DTO
{
    public class PostApproveDTO
    {
        [Required(ErrorMessage = "Vui lòng nhập Id bài viết")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn trạng thái")]
        [EnumValue(typeof(PostStatusEnum))]
        public string Status { get; set; } = null!;
    }
}
