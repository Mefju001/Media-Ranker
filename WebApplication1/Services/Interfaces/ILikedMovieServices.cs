using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.Models;

namespace WebApplication1.Services.Interfaces
{
    public interface ILikedMovieServices
    {
        Task<List<LikedMovieResponse>> GetAllAsync();
        Task<LikedMovieResponse?> GetById(int id);
        Task<(int movieId, LikedMovieResponse response)> Add(LikedMovieRequest movie);
        Task<bool> Delete(int id);
    }
}