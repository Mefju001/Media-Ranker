using Domain.Entity;

namespace Application.Features.GamesServices.GetGamesByCriteria
{
    public interface IGameSortAndFilterService
    {
        Task<IQueryable<Game>> GetGamesByCriteriaAsync(GetGamesByCriteriaQuery request);
    }
}
