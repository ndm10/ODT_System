using ODT_System.Models;
using ODT_System.Repository.Interface;

namespace ODT_System.Repository
{
    public class StudyTimeRepository : BaseRepository<StudyTime>, IStudyTimeRepository
    {
        public StudyTimeRepository(OdtsystemContext context) : base(context)
        {
        }
    }
}
