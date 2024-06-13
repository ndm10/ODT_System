using ODT_System.Models;

namespace ODT_System.Repository.Interface
{
    public interface IPostRepository : IBaseRepository<Post>
    {
        public Post? FindByIdIncludeStudyTimes(int id);
        public Post? FindByIdIncludeAll(int id);

    }
}
