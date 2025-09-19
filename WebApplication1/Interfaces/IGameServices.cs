using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;

namespace WebApplication1.Interfaces
{
    public interface IGameServices
    {
        Task<List<GameResponse>> GetAllAsync();
        Task<List<GameResponse>> GetSortAll(string sort);
        Task<List<GameResponse>> GetGamesByAvrRating();
        Task<List<GameResponse>> GetGames(string? name, string? genreName);
        Task<GameResponse?> GetById(int id);
        Task<(int movieId, GameResponse response)> Upsert(int? movieId, GameRequest game);
        Task<bool> Delete(int id);
    }
}
