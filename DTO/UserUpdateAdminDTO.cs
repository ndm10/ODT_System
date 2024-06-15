using System.ComponentModel.DataAnnotations;

namespace ODT_System.DTO
{
    public class UserUpdateAdminDTO
    {
        [Required(ErrorMessage = "Vui lòng nhập Id của người dùng")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui chọn trạng thái")]
        public bool IsActive { get; set; }
    }
}
