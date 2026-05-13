using Application.Features.Games.Command;

namespace Application.Features.Games.GetByCriteria
{
    public interface ISortAndFilterService
    {
        Task<List<GameResponse>> GetByCriteriaAsync(GetByCriteriaQuery request, CancellationToken cancellationToken);
    }
}
