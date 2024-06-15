using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ODT_System.DTO;
using ODT_System.Enums;
using ODT_System.Models;
using ODT_System.Repository.Interface;
using ODT_System.Services.Interface;
using ODT_System.SharedObject;

namespace ODT_System.Services
{
    public class AdminService : IAdminService
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public AdminService(IPostRepository postRepository, IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _postRepository = postRepository;
            _userRepository = userRepository;
        }

        public bool AppovePost(PostApproveDTO postApproveDTO, out string message)
        {
            // Find post by id
            var post = _postRepository.FindByIdIncludeAll(postApproveDTO.Id);

            // Check post is exist
            if (post == null)
            {
                message = "Bài đăng không tồn tại";
                return false;
            }

            // Update status
            post.Status = postApproveDTO.Status;

            // Update Post
            _postRepository.Update(post);
            _postRepository.Save();

            message = "Duyệt bài đăng thành công";
            return true;
        }

        public PaginatedModel<PostAdminDTO> GetAllPost(int? pageIndex, int? pageSize, string? status, string? textSearch)
        {
            // Get list post by user id
            var posts = _postRepository.GetAll().Where(p => p.IsDeleted == false);

            // Filter by status
            if (status != null)
            {
                posts = posts.Where(p => p.Status == status);
            }

            // Filter by text search
            if (!string.IsNullOrEmpty(textSearch))
            {
                var pattern = $"%{textSearch}%";
                posts = posts.Where(p => EF.Functions.Like(p.ShortDescription, pattern)
                                         || EF.Functions.Like(p.Subject, pattern)
                                         || EF.Functions.Like(p.ContactPhone, pattern)
                                         || EF.Functions.Like(p.StudyAddress, pattern));
            }

            // Include other properties
            // Order by Status, UpdatedAt, CreatedAt
            posts = posts.Include(p => p.StudyTimes).OrderBy(p => p.Status != PostStatusEnum.Pending.ToString())
                .ThenByDescending(p => p.UpdatedAt).ThenByDescending(p => p.CreatedAt);

            // Pagination
            var paginatedModel = PaginatedModel<Post>.GetPaging(pageIndex, pageSize, posts);

            // Map Post to PostAdminDTO
            var postDTOs = _mapper.Map<List<PostAdminDTO>>(paginatedModel.items);
            var paginatedModelDTO = new PaginatedModel<PostAdminDTO>
            {
                pageIndex = paginatedModel.pageIndex,
                pageSize = paginatedModel.pageSize,
                totalItems = paginatedModel.totalItems,
                items = postDTOs
            };

            return paginatedModelDTO;
        }

        public PaginatedModel<UserAdminDTO> GetAllUser(int? pageIndex, int? pageSize, bool? status, string? textSearch)
        {
            // Get all user
            var users = _userRepository.GetAll();

            // Filter by status
            if (status != null)
            {
                users = users.Where(u => u.IsActive == status);
            }

            // Filter by text search
            if (!string.IsNullOrEmpty(textSearch))
            {
                var pattern = $"%{textSearch}%";
                users = users.Where(u => EF.Functions.Like(u.FullName, pattern)
                                         || EF.Functions.Like(u.Email, pattern)
                                         || EF.Functions.Like(u.Phone, pattern));
            }

            // Order by IsActive
            users = users.OrderBy(u => u.IsActive == false);

            // Pagination
            var paginatedModel = PaginatedModel<User>.GetPaging(pageIndex, pageSize, users);

            // Map User to UserAdminDTO
            var userDTOs = _mapper.Map<List<UserAdminDTO>>(paginatedModel.items);
            var paginatedModelDTO = new PaginatedModel<UserAdminDTO>
            {
                pageIndex = paginatedModel.pageIndex,
                pageSize = paginatedModel.pageSize,
                totalItems = paginatedModel.totalItems,
                items = userDTOs
            };

            return paginatedModelDTO;
        }

        public PostAdminDTO? GetPostDetails(int id)
        {
            var post = _postRepository.FindByIdIncludeAll(id);

            // Map Post to PostAdminDTO
            var postDTO = _mapper.Map<PostAdminDTO>(post);

            return postDTO;
        }

        public UserAdminDTO GetUserDetails(int id)
        {
            // Find user by id
            var user = _userRepository.FindById(id);

            // Map User to UserAdminDTO
            var userDTO = _mapper.Map<UserAdminDTO>(user);

            return userDTO;
        }

        public bool UpdateUserStatus(UserUpdateAdminDTO userUpdateAdminDTO, out string message)
        {
            // Find user by id
            var user = _userRepository.FindById(userUpdateAdminDTO.Id);

            // Check user is exist
            if (user == null)
            {
                message = "Người dùng không tồn tại";
                return false;
            }

            // Update status
            user.IsActive = userUpdateAdminDTO.IsActive;

            // Update User
            _userRepository.Update(user);
            _userRepository.Save();

            message = "Cập nhật trạng thái người dùng thành công";
            return true;
        }
    }
}
