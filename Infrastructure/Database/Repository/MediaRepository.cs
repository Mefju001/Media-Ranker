using Application.Common.Interfaces;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace Infrastructure.Database.Repository
{
    public class MediaRepository<T> : Repository<T, int>, IMediaRepository<T> where T : Media
    {
        public MediaRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        Task<Dictionary<int, T>> IMediaRepository<T>.GetByIds(List<int> mediaIds, CancellationToken cancellationToken)
        {
            var medias = appDbContext.Set<T>()
                .Where(m => mediaIds.Contains(m.Id))
                .AsNoTrackingWithIdentityResolution()
                .ToDictionaryAsync(m => m.Id, m => m, cancellationToken);
            return medias;
        }

        public async Task<List<T>> FromAsQueryableToList(IQueryable<T> query, CancellationToken cancellationToken)
        {
            return await query.AsNoTracking().ToListAsync(cancellationToken);
        }
        public async Task<List<Review>> GetAllReviewsAsync(CancellationToken cancellationToken)
        {
            return await appDbContext.Set<T>()
                .SelectMany(m => m.Reviews)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<Review?> GetReviewByIdAsync(int reviewId, CancellationToken cancellationToken)
        {
            return await appDbContext.Set<T>()
                .SelectMany(m => m.Reviews)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == reviewId, cancellationToken);
        }

        public Task<List<int>> GetTheLastestReviewAsync(CancellationToken cancellationToken)
        {
            return appDbContext.Set<T>()
                .SelectMany(m => m.Reviews)
                .OrderByDescending(r => r.AuditInfo.CreatedAt)
                .Take(5)
                .Select(r => r.MediaId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<string>> GetTitleByIdsAsync(List<int> reviewsId, CancellationToken cancellationToken)
        {
            return await appDbContext.Set<T>()
                .Where(m=>m.Reviews.Any(r => reviewsId.Contains(r.Id)))
                .Select(m => m.Title)
                .ToListAsync(cancellationToken);
        }
    }
}
