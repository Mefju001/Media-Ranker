using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IGameRepository
    {
        Task AddListOfGames(List<GameDomain> games, CancellationToken cancellationToken);
        Task AddGameAsync(GameDomain game);
        Task<GameDomain?> GetGameDomainAsync(int gameId, CancellationToken cancellationToken);
        Task DeleteGame(GameDomain game);
        Task<List<GameDomain>> GetAllAsync();
        Task<IQueryable<GameDomain>> AsQueryable();
    }
}
