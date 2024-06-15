namespace ODT_System.DTO
{
    public class UserChatDTO
    {
        public int Id { get; set; }

        public string Email { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public bool Gender { get; set; }

        public DateOnly Dob { get; set; }

        public string Phone { get; set; } = null!;

        public int RoleId { get; set; }

        public string LastMessage { get; set; } = null!;

        public DateTime LastMessageTime { get; set; }
    }
}
