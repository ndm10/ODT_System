using Microsoft.EntityFrameworkCore;
using ODT_System.Models;
using ODT_System.Repository.Interface;

namespace ODT_System.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly OdtsystemContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(OdtsystemContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public T? Find(int id)
        {
            return _dbSet.Find(id);
        }
    }
}
