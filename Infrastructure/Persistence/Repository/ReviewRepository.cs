using Application.Common.Interfaces;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Persistence.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext appDbContext;
        public ReviewRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<Review> AddAsync(Review review, CancellationToken cancellationToken)
        {
            var createdReview = await appDbContext.Reviews.AddAsync(review, cancellationToken);
            return createdReview.Entity;
        }
        public Task<Review?> GetReviewByIdAsync(int reviewId, CancellationToken cancellationToken)
        {
            return appDbContext.Reviews.AsNoTracking().FirstOrDefaultAsync(r => r.Id == reviewId, cancellationToken);
        }
        public async Task<List<string>> GetTheLastestReviewAsync(CancellationToken cancellation)
        {
            var media = appDbContext.Medias.AsQueryable();
            return await appDbContext.Reviews
                .AsNoTrackingWithIdentityResolution()
                .OrderByDescending(r => r.AuditInfo.CreatedAt)
                .Take(10)
                .Join(media,
                    review => review.MediaId,
                    media => media.Id,
                    (review, media) => new { Review = review, Media = media })
                .Select(r => r.Media.Title)
                .Distinct()
                .ToListAsync(cancellation);
        }
        public async Task<List<Review>> GetAllReviewsAsync(CancellationToken cancellation)
        {
            return await appDbContext.Reviews.AsNoTrackingWithIdentityResolution()
                .ToListAsync(cancellation);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            await appDbContext.Reviews.Where(r => r.Id == id).ExecuteDeleteAsync(cancellationToken);
        }
    }
}
