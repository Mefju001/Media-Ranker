using Application.Common.Interfaces;
using Application.Features.Common.Interfaces;
using Domain.Aggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repository
{
    public class DirectorRepository(IAppDbContext appDbContext) : Repository<Director, Guid>(appDbContext), IDirectorRepository
    {
        public async Task<Director?> FirstOrDefaultForNameAndSurnameAsync(string name, string surname, CancellationToken cancellationToken)
        {
            return await dbSet.AsNoTracking().FirstOrDefaultAsync(d => d.fullname.FirstName == name && d.fullname.LastName == surname, cancellationToken);
        }
        public async Task<List<Director>> FindByNamesAsync(List<(string, string)> fullnames, CancellationToken cancellationToken)
        {
            var names = fullnames.Select(x => x.Item1).Distinct().ToList();
            var surnames = fullnames.Select(x => x.Item2).Distinct().ToList();
            var results = await dbSet.Where(d => names.Contains(d.fullname.FirstName) && surnames.Contains(d.fullname.LastName)).AsNoTracking().ToListAsync(cancellationToken);
            return results;
        }
        public Task<Dictionary<Guid, Director>> GetDirectorsDictionary(CancellationToken cancellationToken)
        {
            var directorsDict = dbSet.AsNoTracking().ToDictionaryAsync(d => d.Id, d => d, cancellationToken);
            return directorsDict;
        }

        public async Task<Dictionary<Guid, Director>> GetByIds(List<Guid> ids, CancellationToken cancellationToken)
        {
            return await dbSet.Where(d => ids.Distinct().Contains(d.Id)).AsNoTracking().ToDictionaryAsync(d => d.Id, d => d, cancellationToken);
        }
    }
}
