using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;

namespace WebApplication1.Interfaces
{
    public interface ILikedMediaServices
    {
        Task<List<LikedMediaResponse>> GetAllAsync();
        Task<List<LikedMediaResponse>> GetUserLikedMedia(int userId);
        Task<LikedMediaResponse> Add(LikedMediaRequest media);
        Task<bool> Delete(int userId, int mediaId);
    }
}