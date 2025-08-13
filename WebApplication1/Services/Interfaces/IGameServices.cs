using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.Models;

namespace WebApplication1.Services.Interfaces
{
    public interface IGameServices
    {
        Task<List<Game>> GetAllAsync();
        Task<List<Game>> GetSortAll(string sort);
        Task<List<Game>> GetGamesByAvrRating();
        Task<List<Game>> GetGames(string? name, string? genreId, string? directorId, int? movieid);
        Task<Game?> GetById(int id);
        Task<(int movieId, Game response)> Upsert(int? movieId, MovieRequest movie);
        Task<bool> Delete(int id);
    }
}
