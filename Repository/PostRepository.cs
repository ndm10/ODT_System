using Microsoft.EntityFrameworkCore;
using ODT_System.Models;
using ODT_System.Repository.Interface;

namespace ODT_System.Repository
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        public PostRepository(OdtsystemContext context) : base(context)
        {
        }

        public Post? FindByIdIncludeAll(int id)
        {
            return _dbSet.Include(p => p.StudyTimes).Include(p => p.User).FirstOrDefault(p => p.Id == id && p.IsDeleted == false);
        }

        public Post? FindByIdIncludeStudyTimes(int id)
        {
           return _dbSet.Include(p => p.StudyTimes).FirstOrDefault(p => p.Id == id && p.IsDeleted == false);
        }
    }
}
