using Application.Common.Interfaces;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class GenreRepository : IGenreRepository
    {
        private readonly AppDbContext _context;
        public GenreRepository(AppDbContext context)
        {
            _context = context;
        }
        public IQueryable<Genre> GetAllQueryable()
        {
            return _context.Genres.AsQueryable();
        }
        public async Task<Genre?> FirstOrDefaultForNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _context.Genres.FirstOrDefaultAsync(g => g.name.Value == name, cancellationToken);
        }
        public async Task<Genre> AddAsync(Genre genre, CancellationToken cancellationToken)
        {
            var entityEntry = await _context.Genres.AddAsync(genre, cancellationToken);
            return entityEntry.Entity;
        }
        public async Task<Genre?> Get(int id, CancellationToken cancellationToken)
        {
            return await _context.Genres.FindAsync(id, cancellationToken);
        }
        public async Task<Dictionary<int,Genre>> GetByIdsAsync(List<int> ids, CancellationToken cancellationToken)
        {
            return await _context.Genres.Where(g => ids.Contains(g.Id)).ToDictionaryAsync(g=>g.Id,cancellationToken);
        }
        public async Task<List<Genre>> GetByNamesAsync(List<string> names, CancellationToken cancellationToken)
        {
            return await _context.Genres.Where(g => names.Contains(g.name.Value)).ToListAsync(cancellationToken);
        }

        public async Task<List<Genre>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Genres.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<int?> GetGenreIdByNameAsync(string name, CancellationToken cancellationToken)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(g => g.name.Value == name, cancellationToken);
            return genre?.Id;
        }

        public async Task<Dictionary<int, Genre>> GetGenresDictionary(CancellationToken cancellationToken)
        {
            var genresDict = await _context.Genres.ToDictionaryAsync(g => g.Id, g => g, cancellationToken);
            return genresDict;
        }
    }
}
