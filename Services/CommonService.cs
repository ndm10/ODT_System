using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ODT_System.DTO;
using ODT_System.Enums;
using ODT_System.Helpers;
using ODT_System.Models;
using ODT_System.Repository.Interface;
using ODT_System.Services.Interface;
using ODT_System.SharedObject;

namespace ODT_System.Services
{
    public class CommonService : ICommonService
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public CommonService(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public PostCommonDTO? GetPostById(int id)
        {
            var post = _postRepository.FindByIdIncludeStudyTimes(id);

            if (post == null)
            {
                return null;
            }

            if(post.IsHidden == true || post.Status != PostStatusEnum.Approved.ToString())
            {
                return null;
            }

            // Mapping
            var postCommonDTO = _mapper.Map<PostCommonDTO>(post);
            return postCommonDTO;
        }

        public PaginatedModel<PostCommonDTO> GetPosts(int? pageIndex, int? pageSize, string? textSearch)
        {
            // Get all posts
            var posts = _postRepository.GetAll().Where(p => p.IsHidden == false && p.IsDeleted == false && p.Status == PostStatusEnum.Approved.ToString());

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
            posts = posts.Include(p => p.StudyTimes).Include(p => p.User);

            // Paging
            var paginatedModel = PaginatedModel<Post>.GetPaging(pageIndex, pageSize, posts);

            // Mapping
            var postCommonDTOs = _mapper.Map<List<PostCommonDTO>>(paginatedModel.items);
            var paginatedModelDTO = new PaginatedModel<PostCommonDTO>
            {
                pageIndex = paginatedModel.pageIndex,
                pageSize = paginatedModel.pageSize,
                totalItems = paginatedModel.totalItems,
                items = postCommonDTOs
            };

            return paginatedModelDTO;

        }
    }
}
