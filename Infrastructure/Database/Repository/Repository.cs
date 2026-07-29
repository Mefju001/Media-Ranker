using Application.Common.Interfaces;
using Application.Features.Common.Interfaces;
using Domain.Base;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Database.Repository
{
    public class Repository<T, TId>(IAppDbContext appDbContext) : IRepository<T, TId> where T : AggregateRoot<TId>
    {
        protected DbSet<T> dbSet = appDbContext.Set<T>();
        public virtual async Task<T> AddAsync(T entity, CancellationToken ct)
        {
            var result = await dbSet.AddAsync(entity, ct);
            return result.Entity;
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct)
        {
            await dbSet.AddRangeAsync(entities, ct);
        }

        public virtual async Task<bool> ExistById(TId id, CancellationToken cancellationToken)
        {
            return await dbSet.AnyAsync(x => x.Id!.Equals(id), cancellationToken);
        }

        public virtual async Task<List<T>> GetAllAsync(CancellationToken ct)
        {
            return await dbSet.ToListAsync(ct);
        }

        public virtual IQueryable<T> GetAsQueryable()
        {
            return dbSet.AsQueryable();
        }

        public virtual async Task<T?> GetByIdAsync(TId id, CancellationToken ct)
        {
            return await dbSet.FirstOrDefaultAsync(x => x.Id!.Equals(id), ct);
        }

        public virtual void Remove(T entity)
        {
            dbSet.Remove(entity);
        }
    }
}
