using Application.Common.DTO.Response;

namespace Application.Features.GamesServices.GetGamesByCriteria
{
    public interface IGameSortAndFilterService
    {
        Task<List<GameResponse>> GetGamesByCriteriaAsync(GetGamesByCriteriaQuery request, CancellationToken cancellationToken);
    }
}
