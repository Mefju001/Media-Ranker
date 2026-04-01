using Domain.Aggregate;
using Domain.Entity;

namespace Application.Common.Interfaces
{
    public interface IMediaRepository<T>:IRepository<T,int>where T : Media
    {
        Task<List<T>> FromAsQueryableToList(IQueryable<T> query, CancellationToken cancellationToken);
        Task<Dictionary<int, T>> GetByIds(List<int> mediaIds, CancellationToken cancellationToken);
        Task<List<Review>> GetAllReviewsAsync(CancellationToken cancellationToken);
        Task<Review?> GetReviewByIdAsync(int reviewId, CancellationToken cancellationToken);
        Task<List<int>> GetTheLastestReviewAsync(CancellationToken cancellationToken);
        Task<List<string>> GetTitleByIdsAsync(List<int> reviewsId, CancellationToken cancellationToken);
    }
}
