using Application.Common.Interfaces;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class GenreRepository : IGenreRepository
    {
        private readonly AppDbContext context;
        public GenreRepository(AppDbContext context)
        {
            this.context = context;
        }
        public IQueryable<Genre> GetAllQueryable()
        {
            return context.Genres.AsQueryable();
        }
        public async Task<Genre?> FirstOrDefaultForNameAsync(string name, CancellationToken cancellationToken)
        {
            return await context.Genres.FirstOrDefaultAsync(g => g.Name.Value == name, cancellationToken);
        }
        public async Task<Genre> AddAsync(Genre genre, CancellationToken cancellationToken)
        {
            var entityEntry = await context.Genres.AddAsync(genre, cancellationToken);
            return entityEntry.Entity;
        }
        public async Task<Genre?> Get(int id, CancellationToken cancellationToken)
        {
            return await context.Genres.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
        }
        public async Task<Dictionary<int, Genre>> GetByIdsAsync(List<int> ids, CancellationToken cancellationToken)
        {
            return await context.Genres.AsNoTracking().Where(g => ids.Contains(g.Id)).ToDictionaryAsync(g => g.Id, cancellationToken);
        }
        public async Task<List<Genre>> GetByNamesAsync(List<string> names, CancellationToken cancellationToken)
        {
            return await context.Genres.AsNoTracking().Where(g => names.Contains(g.Name.Value)).ToListAsync(cancellationToken);
        }

        public async Task<List<Genre>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.Genres.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<int?> GetGenreIdByNameAsync(string name, CancellationToken cancellationToken)
        {
            var genreId = await context.Genres.AsNoTracking().Where(g => g.Name.Value == name).Select(g => (int?)g.Id).FirstOrDefaultAsync(cancellationToken);
            return genreId;
        }

        public async Task<Dictionary<int, Genre>> GetGenresDictionary(CancellationToken cancellationToken)
        {
            var genresDict = await context.Genres.AsNoTracking().ToDictionaryAsync(g => g.Id, g => g, cancellationToken);
            return genresDict;
        }
    }
}
