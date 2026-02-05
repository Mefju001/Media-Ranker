using Application.Common.Interfaces;
using Domain.Entity;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Persistence.Repository
{
    public class ReviewRepository:IReviewRepository
    {
        private readonly AppDbContext appDbContext;
        public ReviewRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<ReviewDomain> AddAsync(ReviewDomain review)
        {
            var createdReview = await appDbContext.Reviews.AddAsync(review);
            return createdReview.Entity;
        }

        public Task<ReviewDomain?> GetReviewByIdAsync(int reviewId)
        {
            return appDbContext.Reviews
                .FirstOrDefaultAsync(r => r.Id == reviewId);
        }
        public async Task<List<string>> GetTheLastestReviewAsync(CancellationToken cancellation)
        {
            var media = appDbContext.Medias.AsQueryable();
            return await appDbContext.Reviews
                .OrderByDescending(r => r.CreatedAt)
                .Take(10)
                .Join(media,
                    review => review.MediaId,
                    media => media.Id,
                    (review, media) => new { Review = review, Media = media })
                .Select(r => r.Media.Title)
                .Distinct()
                .ToListAsync(cancellation);
        }
        public async Task<List<ReviewDomain>> GetAllReviewsAsync(CancellationToken cancellation)
        {
            return await appDbContext.Reviews
                .ToListAsync(cancellation);
        }

        public Task DeleteAsync(ReviewDomain review)
        {
            appDbContext.Reviews.Remove(review);
            return Task.CompletedTask;
        }
    }
}
