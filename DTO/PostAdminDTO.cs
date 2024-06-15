using ODT_System.Models;

namespace ODT_System.DTO
{
    public class PostAdminDTO
    {
        public int Id { get; set; }

        public string ContactPhone { get; set; } = null!;

        public string ShortDescription { get; set; } = null!;

        public string StudyAddress { get; set; } = null!;

        public int NumberOfStudent { get; set; }

        public DateOnly StartDate { get; set; }

        public decimal? StudyHour { get; set; }

        public string Subject { get; set; } = null!;

        public string StudentGender { get; set; } = null!;

        public decimal? Fee { get; set; }

        public string? TypeOfFee { get; set; }

        public int UserId { get; set; }

        public string Description { get; set; } = null!;

        public string Status { get; set; } = null!;

        public bool IsHidden { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public ICollection<StudyTimeDTO> StudyTimes { get; set; } = new List<StudyTimeDTO>();

        public UserAdminDTO User { get; set; } = null!;
    }
}
