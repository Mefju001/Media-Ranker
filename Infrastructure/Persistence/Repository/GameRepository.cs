using Application.Common.Interfaces;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Persistence.Repository
{
    public class GameRepository : IGameRepository
    {
        private readonly AppDbContext _context;
        public GameRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Game>> GetListFromQueryAsync(IQueryable<Game> query, CancellationToken cancellationToken)
        {
            return await query.ToListAsync(cancellationToken);
        }
        public async Task AddListOfGames(List<Game> games, CancellationToken cancellationToken)
        {
            await _context.Games.AddRangeAsync(games, cancellationToken);
        }
        public async Task<Game?> GetGameDomainAsync(int gameId, CancellationToken cancellationToken)
        {
            return await _context.Games.FirstOrDefaultAsync(g => g.Id == gameId, cancellationToken);
        }
        public async Task DeleteGame(Game game)
        {
            _context.Games.Remove(game);
        }
        public async Task<Game> AddGameAsync(Game game)
        {
            var createdGame = await _context.Games.AddAsync(game);
            return createdGame.Entity;
        }
        public async Task<List<Game>> GetAllAsync()
        {
            return await _context.Games.AsNoTracking().ToListAsync();
        }

        public IQueryable<Game> AsQueryable()
        {
            return _context.Games
                .Include(g => g.Stats)
                .AsNoTracking()
                .AsQueryable();
        }
    }
}
