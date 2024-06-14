using ODT_System.Enums;
using ODT_System.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ODT_System.DTO
{
    public class StudyTimeCreateDTO
    {
        [Required(ErrorMessage = "Vui lòng nhập thời gian bắt đầu")]
        [TimeBefore("To", ErrorMessage = "Thời gian bắt đầu trước thời gian kết thúc")]
        public TimeOnly? From { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập thời gian kết thúc")]
        public TimeOnly? To { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ngày trong tuần")]
        [EnumValue(typeof(DayOfWeekEnum))]
        public string? DayOfWeek { get; set; }
    }
}
