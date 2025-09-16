using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.Models;

namespace WebApplication1.Services.Interfaces
{
    public interface ILikedMediaServices
    {
        Task<List<LikedMedia>> GetAllAsync();
        Task<List<LikedMedia>> GetUserLikedMedia(int userId);
        Task<LikedMediaResponse?> GetById(int id);
        Task<LikedMediaResponse> Add(LikedMediaRequest media);
        Task<bool> Delete(int userId, int mediaId);
    }
}