using Domain.Entity;

namespace Application.Common.Interfaces
{
    public interface IGameRepository
    {
        Task<List<GameDomain>> GetListFromQueryAsync(IQueryable<GameDomain> query, CancellationToken cancellationToken);
        Task AddListOfGames(List<GameDomain> games, CancellationToken cancellationToken);
        Task AddGameAsync(GameDomain game);
        Task<GameDomain?> GetGameDomainAsync(int gameId, CancellationToken cancellationToken);
        Task DeleteGame(GameDomain game);
        Task<List<GameDomain>> GetAllAsync();
        Task<IQueryable<GameDomain>> AsQueryable();
    }
}
