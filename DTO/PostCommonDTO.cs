using ODT_System.Models;

namespace ODT_System.DTO
{
    public class PostCommonDTO
    {
        public int Id { get; set; }

        public string ContactPhone { get; set; } = null!;

        public string ShortDescription { get; set; } = null!;

        public string StudyAddress { get; set; } = null!;

        public int NumberOfStudent { get; set; }

        public DateOnly StartDate { get; set; }

        public decimal? StudyHour { get; set; }

        public string Subject { get; set; } = null!;

        public string? StudentGender { get; set; }

        public decimal? Fee { get; set; }

        public string? TypeOfFee { get; set; }

        public int UserId { get; set; }

        public string Description { get; set; } = null!;

        public virtual ICollection<StudyTimeDTO> StudyTimes { get; set; } = new List<StudyTimeDTO>();

        public virtual UserCommonDTO User { get; set; } = null!;
    }
}
