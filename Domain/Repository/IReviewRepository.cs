using Domain.Entity;

namespace Application.Common.Interfaces
{
    public interface IReviewRepository
    {
        Task<Review?> GetReviewByIdAsync(int reviewId, CancellationToken cancellationToken);
        Task<Review> AddAsync(Review review, CancellationToken cancellationToken);
        Task<List<string>> GetTheLastestReviewAsync(CancellationToken cancellationToken);
        Task<List<Review>> GetAllReviewsAsync(CancellationToken cancellation);
        Task DeleteAsync(Review review);
    }
}
