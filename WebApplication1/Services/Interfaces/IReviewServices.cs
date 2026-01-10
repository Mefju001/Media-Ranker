using WebApplication1.Application.Common.DTO.Request;
using WebApplication1.Application.Common.DTO.Response;

namespace WebApplication1.Services.Interfaces
{
    public interface IReviewServices
    {
        Task<List<ReviewResponse>> GetAllAsync();
        Task<List<string>> GetTheLatestReviews();
        Task<ReviewResponse?> GetById(int id);
        Task<bool> Delete(int id, int userId);
        Task<ReviewResponse> Upsert(int? reviewId, int userId, int movieId, ReviewRequest reviewRequest);
    }
}
