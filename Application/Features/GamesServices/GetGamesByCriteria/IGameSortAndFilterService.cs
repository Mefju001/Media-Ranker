using Application.Common.DTO.Response;
using Domain.Aggregate;

namespace Application.Features.GamesServices.GetGamesByCriteria
{
    public interface IGameSortAndFilterService
    {
        Task<List<GameResponse>> GetGamesByCriteriaAsync(GetGamesByCriteriaQuery request, CancellationToken cancellationToken);
    }
}
