using ODT_System.Models;

namespace ODT_System.DTO
{
    public class ViewProfileDTO
    {
        public string Email { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public bool Gender { get; set; }

        public DateOnly Dob { get; set; }

        public string Phone { get; set; } = null!;

        public int RoleId { get; set; }

        public string? Desciption { get; set; }
    }
}
