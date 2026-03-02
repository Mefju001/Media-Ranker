using Domain.Entity;

namespace Application.Common.Interfaces
{
    public interface IGameRepository
    {
        Task<List<Game>> GetListFromQueryAsync(IQueryable<Game> query, CancellationToken cancellationToken);
        Task AddListOfGames(List<Game> games, CancellationToken cancellationToken);
        Task AddGameAsync(Game game);
        Task<Game?> GetGameDomainAsync(int gameId, CancellationToken cancellationToken);
        Task DeleteGame(Game game);
        Task<List<Game>> GetAllAsync();
        IQueryable<Game> AsQueryable();
    }
}
