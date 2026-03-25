using Application.Common.Interfaces;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Persistence.Repository
{
    public class GameRepository : IGameRepository
    {
        private readonly AppDbContext context;
        public GameRepository(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<List<Game>> GetListFromQueryAsync(IQueryable<Game> query, CancellationToken cancellationToken)
        {
            return await query.AsNoTrackingWithIdentityResolution().ToListAsync(cancellationToken);
        }
        public async Task AddListOfGames(List<Game> games, CancellationToken cancellationToken)
        {
            await context.Games.AddRangeAsync(games, cancellationToken);
        }
        public async Task<Game?> GetGameDomainAsync(int gameId, CancellationToken cancellationToken)
        {
            return await context.Games.Include(g => g.Stats).FirstOrDefaultAsync(g => g.Id == gameId, cancellationToken);
        }
        public void DeleteGame(Game game)
        {
            context.Games.Remove(game);
        }
        public async Task<Game> AddGameAsync(Game game, CancellationToken cancellationToken)
        {
            var createdGame = await context.Games.AddAsync(game, cancellationToken);
            return createdGame.Entity;
        }
        public async Task<List<Game>> GetAllAsync(CancellationToken cancellation)
        {
            return await context.Games.AsNoTrackingWithIdentityResolution().ToListAsync(cancellation);
        }
        public IQueryable<Game> AsQueryable()
        {
            return context.Games.AsQueryable();
        }
    }
}
