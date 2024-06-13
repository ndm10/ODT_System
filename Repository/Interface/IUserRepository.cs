using ODT_System.Models;

namespace ODT_System.Repository.Interface
{
    public interface IUserRepository: IBaseRepository<User>
    {
        public User? FindByEmail(string email);
        public User? FindByEmailIncludeRole(string email);
        public User Create(User user);
        public User Update(User user);
        public void Delete(int userId);
        public void DeleteAll();
        public User? FindById(int userId);
        public User? FindByName(string name);
    }
}
