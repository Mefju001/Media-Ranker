using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.Models;

namespace WebApplication1.Services.Interfaces
{
    public interface IGameServices
    {
        Task<List<GameResponse>> GetAllAsync();
        Task<List<GameResponse>> GetSortAll(string sort);
        Task<List<GameResponse>> GetGamesByAvrRating();
        Task<List<GameResponse>> GetGames(string? name, string? genreName, int? movieid);
        Task<GameResponse?> GetById(int id);
        Task<(int movieId, Game response)> Upsert(int? movieId, MovieRequest movie);
        Task<bool> Delete(int id);
    }
}
