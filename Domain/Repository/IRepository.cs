using Domain.Base;

namespace Application.Common.Interfaces
{
    public interface IRepository<T, TId> where T : AggregateRoot<TId>
    {
        Task<T?> GetByIdAsync(TId id, CancellationToken ct);
        Task<bool> ExistById(TId id, CancellationToken cancellationToken);
        Task<List<T>> GetAllAsync(CancellationToken ct);
        Task<T> AddAsync(T entity, CancellationToken ct);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct);
        IQueryable<T> GetAsQueryable();
        void Update(T entity);
        void Remove(T entity);
    }
}
