using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.Models;

namespace WebApplication1.Services.Interfaces
{
    public interface ILikedMediaServices
    {
        Task<List<LikedMediaResponse>> GetAllAsync();
        Task<List<LikedMediaResponse>> GetUserLikedMedia(int userId);
        Task<LikedMediaResponse> Add(LikedMediaRequest media);
        Task<bool> Delete(int userId, int mediaId);
    }
}