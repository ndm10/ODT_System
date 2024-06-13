using ODT_System.Models;
using ODT_System.Repository.Interface;
using Microsoft.EntityFrameworkCore;
namespace ODT_System.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly OdtsystemContext _context;

        public UserRepository(OdtsystemContext context) : base(context)
        {
            _context = context;
        }

        public User Create(User user)
        {
            return _context.Users.Add(user).Entity;
        }

        public void Delete(int userId)
        {
            var user = FindById(userId);
            if (user != null)
                _context.Users.Remove(user);
        }

        public void DeleteAll()
        {
            _context.Users.RemoveRange(_context.Users);
        }

        public User? FindByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public User? FindByEmailIncludeRole(string email)
        {
            return _context.Users.Include(u => u.Role).FirstOrDefault(u => u.Email == email);
        }

        public User? FindById(int userId)
        {
            return _context.Users.Find(userId);
        }

        public User? FindByName(string name)
        {
            return _context.Users.FirstOrDefault(u => u.FullName == name);
        }

        public User Update(User user)
        {
            return _context.Users.Update(user).Entity;
        }
    }
}
