namespace ODT_System.Repository.Interface
{
    public interface IBaseRepository<T> where T : class
    {
        public IQueryable<T> GetAll();
        public void Add(T entity);
        public void Update(T entity);
        public void Delete(T entity);
        public T? Find(int id);
        public void Save();
    }
}
