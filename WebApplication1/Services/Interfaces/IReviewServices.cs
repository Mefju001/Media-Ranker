using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;

namespace WebApplication1.Services.Interfaces
{
    public interface IReviewServices
    {
        Task<List<ReviewResponse>> GetAllAsync();
        Task<List<ReviewResponse>> GetTheLatestReviews();
        Task<ReviewResponse> GetById(int id);
        Task<bool> Delete(int id);
        Task<(int reviewId, ReviewResponse response)> Upsert(int? reviewId, int userId, int movieId, ReviewRequest reviewRequest);
    }
}
