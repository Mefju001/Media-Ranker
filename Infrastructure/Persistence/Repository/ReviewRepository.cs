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
            return await appDbContext.Reviews
                .Include(r => r.User)
                .Include(r => r.Media)
                .OrderByDescending(r => r.CreatedAt)
                .Take(10)
                .Select(r => r.Media.Title)
                .Distinct()
                .ToListAsync(cancellation);
        }
        public async Task<List<ReviewDomain>> GetAllReviewsAsync(CancellationToken cancellation)
        {
            return await appDbContext.Reviews
                .Include(r => r.User)
                .Include(r => r.Media)
                .ToListAsync(cancellation);
        }

        public Task DeleteAsync(ReviewDomain review)
        {
            appDbContext.Reviews.Remove(review);
            return Task.CompletedTask;
        }
    }
}
