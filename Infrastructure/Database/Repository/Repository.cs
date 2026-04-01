using Application.Common.Interfaces;
using Domain.Base;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Database.Repository
{
    public class Repository<T,TId> : IRepository<T,TId> where T : AggregateRoot<TId>
    {
        protected readonly AppDbContext appDbContext;
        public Repository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<T> AddAsync(T entity, CancellationToken ct)
        {
            var result = await appDbContext.Set<T>().AddAsync(entity,ct);
            return result.Entity;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct)
        {
            await appDbContext.Set<T>().AddRangeAsync(entities, ct);
        }

        public async Task<List<T>> GetAllAsync(CancellationToken ct)
        {
            return await appDbContext.Set<T>().ToListAsync(ct);
        }

        public IQueryable<T> GetAsQueryable()
        {
            return appDbContext.Set<T>().AsQueryable();
        }

        public async Task<T?> GetByIdAsync(TId id, CancellationToken ct)
        {
            return await appDbContext.Set<T>().FirstOrDefaultAsync(x=>x.Id!.Equals(id), ct);
        }

        public void Remove(T entity)
        {
            appDbContext.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            appDbContext.Set<T>().Update(entity);
        }
    }
}
