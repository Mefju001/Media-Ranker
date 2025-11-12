using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.QueryHandler.Query;

namespace WebApplication1.Services.Interfaces
{
    public interface IGameServices
    {
        Task<List<GameResponse>> GetAllAsync();
        Task<List<GameResponse>> GetSortAll(string isDesceding,string sortByField);
        Task<List<GameResponse>> GetGamesByAvrRating();
        Task<List<GameResponse>> GetGamesByCriteriaAsync(GameQuery gameQuery);
        Task<GameResponse?> GetById(int id);
        Task<(int movieId, GameResponse response)> Upsert(int? movieId, GameRequest game);
        Task<bool> Delete(int id);
    }
}
