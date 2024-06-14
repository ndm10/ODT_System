namespace ODT_System.DTO
{
    public class StudyTimeDTO
    {
        public int Id { get; set; }

        public int PostId { get; set; }

        public TimeOnly? From { get; set; }

        public TimeOnly? To { get; set; }

        public string? DayOfWeek { get; set; }
    }
}
