using Application.Features.Common.Interfaces;
using Domain.Aggregate;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repository
{
    public class GenreRepository(IAppDbContext appDbContext) : Repository<Genre, Guid>(appDbContext), IGenreRepository
    {

        public async Task<Genre?> FirstOrDefaultForNameAsync(string name, CancellationToken cancellationToken)
        {
            return await dbSet.FirstOrDefaultAsync(g => g.Name == name, cancellationToken);
        }
        public async Task<Dictionary<Guid, Genre>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken)
        {
            return await dbSet.AsNoTracking().Where(g => ids.Contains(g.Id)).ToDictionaryAsync(g => g.Id, cancellationToken);
        }
        public async Task<List<Genre>> GetByNamesAsync(List<string> names, CancellationToken cancellationToken)
        {
            return await dbSet.AsNoTracking().Where(g => names.Contains(g.Name)).ToListAsync(cancellationToken);
        }
        public async Task<Dictionary<Guid, Genre>> GetGenresDictionary(CancellationToken cancellationToken)
        {
            var genresDict = await dbSet.AsNoTracking().ToDictionaryAsync(g => g.Id, g => g, cancellationToken);
            return genresDict;
        }
    }
}
