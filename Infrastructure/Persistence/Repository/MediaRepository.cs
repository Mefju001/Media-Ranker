using Application.Common.Interfaces;
using Domain.Aggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class MediaRepository : IMediaRepository
    {
        private readonly AppDbContext appDbContext;

        public MediaRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public Task<Dictionary<int, Media>> GetByIds(List<int> mediaIds, CancellationToken cancellationToken)
        {
            var medias = appDbContext.Medias
                .Where(m => mediaIds.Contains(m.Id))
                .AsNoTrackingWithIdentityResolution()
                .ToDictionaryAsync(m => m.Id, m => m, cancellationToken);
            return medias;
        }

        public async Task<Media?> GetMediaById(int mediaId, CancellationToken cancellationToken)
        {
            var media = await appDbContext.Medias
                .Where(m => m.Id == mediaId)
                .Include(r => r.Reviews)
                .FirstOrDefaultAsync(cancellationToken);
            return media;
        }

        public async Task<List<T>> GetByTypeAsync<T>(CancellationToken ct) where T : Media
        {
            return await appDbContext.Set<T>().ToListAsync(ct);
        }

        public async Task<T?> GetByIdWithDetailsAsync<T>(int id, CancellationToken ct) where T : Media
        {
            return await appDbContext.Set<T>()
                .Include(m => m.Reviews)
                .Include(m=>m.LikedMedia)
                .FirstOrDefaultAsync(m => m.Id == id, ct);
        }

        public async Task<Media?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await appDbContext.Set<Media>().FirstOrDefaultAsync(m => m.Id == id, ct);
        }

        public async Task<List<Media>> GetAllAsync(CancellationToken ct)
        {
            return await appDbContext.Set<Media>().ToListAsync(ct);
        }

        public async Task AddAsync(Media entity, CancellationToken ct)
        {
            await appDbContext.Set<Media>().AddAsync(entity, ct);
        }

        public async Task AddRangeAsync(IEnumerable<Media> entities, CancellationToken ct)
        {
            await appDbContext.Set<Media>().AddRangeAsync(entities, ct);
        }

        public void Update(Media entity)
        {
            //appDbContext.Set<Media>().;
        }

        public void Remove(Media entity)
        {
            appDbContext.Set<Media>().Remove(entity);
        }

        public async Task<T?> GetByIdAsync<T>(int id, CancellationToken ct) where T : Media
        {
            return await appDbContext.Set<T>().FirstOrDefaultAsync(m => m.Id == id, ct);
        }

        public async Task<T> AddAsync<T>(T entity, CancellationToken ct) where T : Media
        {
            var result = await appDbContext.Set<T>().AddAsync(entity,ct);
            return result.Entity;
        }

        public Task<List<T>> GetAll<T>(CancellationToken cancellationToken) where T : Media
        {
            return appDbContext.Set<T>().ToListAsync(cancellationToken);
        }

        public IQueryable<T> GetAsQueryable<T>() where T : Media
        {
            return appDbContext.Set<T>().AsQueryable();
        }

        public async Task<List<T>> FromAsQueryableToList<T>(IQueryable<T>query, CancellationToken cancellationToken) where T : Media
        {
            return await query.AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}
