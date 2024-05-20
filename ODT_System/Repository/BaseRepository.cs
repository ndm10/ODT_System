using ODT_System.Models;
using ODT_System.Repository.Interface;

namespace ODT_System.Repository
{
    public class BaseRepository: IBaseRepository
    {
        private readonly OdtsystemContext _context;

        public BaseRepository(OdtsystemContext context)
        {
            _context = context;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
