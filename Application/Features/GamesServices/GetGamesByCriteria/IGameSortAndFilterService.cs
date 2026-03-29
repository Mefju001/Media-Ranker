using Domain.Aggregate;

namespace Application.Features.GamesServices.GetGamesByCriteria
{
    public interface IGameSortAndFilterService
    {
        IQueryable<Game> GetGamesByCriteriaAsync(GetGamesByCriteriaQuery request);
    }
}
