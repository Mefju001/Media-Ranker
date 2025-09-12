using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.Models;

namespace WebApplication1.Services.Interfaces
{
    public interface ILikedMediaServices
    {
        Task<List<LikedMedia>> GetAllAsync();
        Task<List<Object>> GetUserLikedMedia(int userId);
        Task<LikedMediaResponse?> GetById(int id);
        Task<(int movieId, LikedMediaResponse response)> Add(LikedMovieRequest movie);
        Task<bool> Delete(int id);
    }
}