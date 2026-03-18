using Domain.Entity;

namespace Application.Common.Interfaces
{
    public interface IGameRepository
    {
        Task<List<Game>> GetListFromQueryAsync(IQueryable<Game> query, CancellationToken cancellationToken);
        Task AddListOfGames(List<Game> games, CancellationToken cancellationToken);
        Task<Game> AddGameAsync(Game game, CancellationToken cancellationToken);
        Task<Game?> GetGameDomainAsync(int gameId, CancellationToken cancellationToken);
        void DeleteGame(Game game);
        Task<List<Game>> GetAllAsync(CancellationToken cancellationToken);
        IQueryable<Game> AsQueryable();
    }
}
