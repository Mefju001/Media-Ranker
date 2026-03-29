namespace Application.Common.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id, CancellationToken ct);
        Task<List<T>> GetAllAsync(CancellationToken ct);
        Task AddAsync(T entity, CancellationToken ct);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct);
        void Update(T entity);
        void Remove(T entity);
    }
}
