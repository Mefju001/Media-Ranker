using Application.Common.Interfaces;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repository
{
    public class GameRepository:IGameRepository
    {
        private readonly AppDbContext _context;
        public GameRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task <List<GameDomain>> GetListFromQueryAsync(IQueryable<GameDomain> query, CancellationToken cancellationToken)
        {
            return await query.ToListAsync(cancellationToken);
        }
        public async Task AddListOfGames(List<GameDomain>games,CancellationToken cancellationToken)
        {
            await _context.Games.AddRangeAsync(games,cancellationToken); 
        }
        public async Task<GameDomain?> GetGameDomainAsync(int gameId,CancellationToken cancellationToken)
        {
            return await _context.Games.FirstOrDefaultAsync(g => g.Id == gameId,cancellationToken);
        }
        public async Task DeleteGame(GameDomain game)
        {
             _context.Games.Remove(game);
        }
        public async Task AddGameAsync(GameDomain game)
        {
            await _context.Games.AddAsync(game);
        }
        public async Task<List<GameDomain>> GetAllAsync()
        {
            return await _context.Games.ToListAsync();
        }

        public async Task<IQueryable<GameDomain>> AsQueryable()
        {
            return _context.Games
                .Include(g => g.Stats)
                .AsNoTracking()
                .AsQueryable();
        }
    }
}
