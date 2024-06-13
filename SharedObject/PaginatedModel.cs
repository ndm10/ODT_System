namespace ODT_System.SharedObject
{
    public class PaginatedModel<T>
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public int totalItems { get; set; }
        public int totalPages
        {
            get
            {
                return (int)Math.Ceiling((double)totalItems / pageSize);
            }
        }
        public object? items { get; set; }

        public static PaginatedModel<T> GetPaging(int? pageIndex, int? pageSize, IQueryable<T> items)
        {
            return new PaginatedModel<T>()
            {
                pageIndex = pageIndex ?? 1,
                pageSize = pageSize ?? 10,
                totalItems = items.Count(),
                items = items.Skip(((pageIndex ?? 1) - 1) * (pageSize ?? 10)).Take(pageSize ?? 10).ToList()
            };
        }
    }
}
