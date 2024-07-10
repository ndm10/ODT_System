using ODT_System.DTO;
using ODT_System.SharedObject;

namespace ODT_System.Services.Interface
{
    public interface ICommonService
    {
        public UserCommonDTO? FindUserProfile(int email);
        public PostCommonDTO? GetPostById(int id);
        public PaginatedModel<PostCommonDTO> GetPosts(int? pageIndex, int? pageSize, string? textSearch, string? addressSearch);
        public PaginatedModel<UserCommonDTO> GetTutors(int? pageIndex, int? pageSize, string? textSearch);
    }
}
