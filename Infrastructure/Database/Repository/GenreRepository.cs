using Application.Common.Interfaces;
using Domain.Aggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repository
{
    public class GenreRepository : Repository<Genre,int>, IGenreRepository
    {

        public GenreRepository(AppDbContext context) : base(context) { }
  
        public async Task<Genre?> FirstOrDefaultForNameAsync(string name, CancellationToken cancellationToken)
        {
            return await appDbContext.Genres.FirstOrDefaultAsync(g => g.Name.Value == name, cancellationToken);
        }
        public async Task<Dictionary<int, Genre>> GetByIdsAsync(List<int> ids, CancellationToken cancellationToken)
        {
            return await appDbContext.Genres.AsNoTracking().Where(g => ids.Contains(g.Id)).ToDictionaryAsync(g => g.Id, cancellationToken);
        }
        public async Task<List<Genre>> GetByNamesAsync(List<string> names, CancellationToken cancellationToken)
        {
            return await appDbContext.Genres.AsNoTracking().Where(g => names.Contains(g.Name.Value)).ToListAsync(cancellationToken);
        }
        public async Task<Dictionary<int, Genre>> GetGenresDictionary(CancellationToken cancellationToken)
        {
            var genresDict = await appDbContext.Genres.AsNoTracking().ToDictionaryAsync(g => g.Id, g => g, cancellationToken);
            return genresDict;
        }
    }
}
