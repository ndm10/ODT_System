namespace ODT_System.DTO
{
    public class UpdateProfileDTO
    {
        public string? FullName { get; set; }

        public bool? Gender { get; set; }

        public DateOnly? Dob { get; set; }

        public string? Phone { get; set; }

        public string? Avatar { get; set; }

        public string? Desciption { get; set; }
    }
}
