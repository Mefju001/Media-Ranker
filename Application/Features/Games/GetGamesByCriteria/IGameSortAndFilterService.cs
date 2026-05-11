using Application.Features.Games.Command;

namespace Application.Features.Games.GetByCriteria
{
    public interface IGameSortAndFilterService
    {
        Task<List<GameResponse>> GetGamesByCriteriaAsync(GetByCriteriaQuery request, CancellationToken cancellationToken);
    }
}
