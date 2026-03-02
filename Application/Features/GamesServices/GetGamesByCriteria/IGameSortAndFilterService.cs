using Domain.Entity;

namespace Application.Features.GamesServices.GetGamesByCriteria
{
    public interface IGameSortAndFilterService
    {
        IQueryable<Game> GetGamesByCriteriaAsync(GetGamesByCriteriaQuery request);
    }
}
