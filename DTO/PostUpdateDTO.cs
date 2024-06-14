using ODT_System.Enums;
using ODT_System.Models;
using System.ComponentModel.DataAnnotations;

namespace ODT_System.DTO
{
    public class PostUpdateDTO
    {
        public int Id { get; set; }

        [RegularExpression("^\\d{10}$", ErrorMessage = "Vui lòng nhập đúng định dạng số điện thoại Việt Nam")]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại liên hệ")]
        public string ContactPhone { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập tóm tắt mô tả")]
        [MaxLength(255, ErrorMessage = "Tóm tắt mô tả dưới 500 ký tự")]
        public string ShortDescription { get; set; } = null!;

        [MaxLength(255, ErrorMessage = "Địa chỉ học dưới 500 ký tự")]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ học")]
        public string StudyAddress { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập số lượng học sinh")]
        public int NumberOfStudent { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ngày bắt đầu")]
        public DateOnly StartDate { get; set; }

        public decimal? StudyHour { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập môn học")]
        public string Subject { get; set; } = null!;

        [EnumValue(typeof(StudentGenderEnum))]
        public string? StudentGender { get; set; }

        public decimal? Fee { get; set; }

        [EnumValue(typeof(TypeOfFeeEnum))]
        public string? TypeOfFee { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mô tả tuyển sinh")]
        public string Description { get; set; } = null!;

        public bool IsHidden { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn thời gian học")]
        public required ICollection<StudyTimeUpdateDTO> StudyTimes { get; set; }
    }
}
