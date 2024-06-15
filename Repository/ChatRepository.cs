using ODT_System.Models;
using ODT_System.Repository.Interface;

namespace ODT_System.Repository
{
    public class ChatRepository : BaseRepository<Chat>, IChatRepository
    {
        public ChatRepository(OdtsystemContext context) : base(context)
        {

        }
    }
}
