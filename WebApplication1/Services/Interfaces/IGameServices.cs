using WebApplication1.Application.Common.DTO.Request;
using WebApplication1.Application.Common.DTO.Response;

namespace WebApplication1.Services.Interfaces
{
    public interface IGameServices
    {
        Task<List<GameResponse>> GetAllAsync();
       //Task<List<GameResponse>> GetGamesByCriteriaAsync(GameQuery gameQuery);
        Task<GameResponse?> GetById(int id);
        Task<GameResponse> Upsert(int? movieId, GameRequest game);
        Task<bool> Delete(int id);
    }
}
