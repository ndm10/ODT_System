using ODT_System.DTO;
using ODT_System.SharedObject;

namespace ODT_System.Services.Interface
{
    public interface IAdminService
    {
        public bool AppovePost(PostApproveDTO postApproveDTO, out string message);
        public PaginatedModel<PostAdminDTO> GetAllPost(int? pageIndex, int? pageSize, string? status, string? textSearch);
        public PaginatedModel<UserAdminDTO> GetAllUser(int? pageIndex, int? pageSize, bool? status, string? textSearch);
        public PostAdminDTO? GetPostDetails(int id);
        public UserAdminDTO GetUserDetails(int id);
        public bool UpdateUserStatus(UserUpdateAdminDTO userUpdateAdminDTO, out string message);
    }
}
