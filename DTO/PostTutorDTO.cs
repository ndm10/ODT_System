using ODT_System.Models;

namespace ODT_System.DTO
{
    public class PostTutorDTO
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

        public string Description { get; set; } = null!;

        public string? Status { get; set; }

        public bool IsHidden { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<StudyTimeDTO> StudyTimes { get; set; } = new List<StudyTimeDTO>();
    }
}
