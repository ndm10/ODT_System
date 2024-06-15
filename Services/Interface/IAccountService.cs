using ODT_System.DTO;
using ODT_System.Models;
using ODT_System.SharedObject;

namespace ODT_System.Services.Interface
{
    public interface IAccountService
    {
        public bool ChangePassword(ChangePasswordDTO changePasswordDTO, string email, out string message);
        public bool CreatePost(PostCreateDTO postCreateDTO, string userEmail, out string message);
        public ViewProfileDTO? FindUserProfile(string email);
        public PaginatedModel<PostTutorDTO> ListPost(string userEmail, int? pageIndex, int? pageSize, string? status, string? textSearch);
        public bool NewPassword(NewPasswordDTO newPasswordDTO, out string message);
        public PostTutorDTO? GetPostById(int id, string userEmail);
        public bool UpdateProfile(UpdateProfileDTO updateProfileDTO, string emailAccount);
        public bool VerifyEmail(VerifyEmailDTO verifyEmailDTO, out string message);
        public bool UpdatePost(PostUpdateDTO postUpdateDTO, string userEmail, out string message);
        public bool DeletePost(int id, string userEmail, out string message);
        public List<ChatDTO> ListInbox(string userEmail, int withUser);
        public List<UserChatDTO> ListChat(string userEmail);
        public bool TryInbox(ChatInBoxDTO chatInBoxDTO, string userEmail, out string message);
    }
}
