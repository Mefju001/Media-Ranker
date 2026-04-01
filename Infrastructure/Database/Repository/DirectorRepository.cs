using Application.Common.Interfaces;
using Domain.Aggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repository
{
    public class DirectorRepository :Repository<Director,int>, IDirectorRepository
    {
        public DirectorRepository(AppDbContext context) : base(context) { }
        public async Task<Director?> FirstOrDefaultForNameAndSurnameAsync(string name, string surname, CancellationToken cancellationToken)
        {
            return await appDbContext.Directors.AsNoTracking().FirstOrDefaultAsync(d => d.Name == name && d.Surname == surname, cancellationToken);
        }
        public async Task<List<Director>> findByNames(List<(string, string)> fullnames, CancellationToken cancellationToken)
        {
            var names = fullnames.Select(x => x.Item1).Distinct().ToList();
            var surnames = fullnames.Select(x => x.Item2).Distinct().ToList();
            var results = await appDbContext.Directors.Where(d => names.Contains(d.Name) && surnames.Contains(d.Surname)).AsNoTracking().ToListAsync(cancellationToken);
            return results;
        }
        public Task<Dictionary<int, Director>> GetDirectorsDictionary(CancellationToken cancellationToken)
        {
            var directorsDict = appDbContext.Directors.AsNoTracking().ToDictionaryAsync(d => d.Id, d => d, cancellationToken);
            return directorsDict;
        }

        public async Task<Dictionary<int, Director>> GetByIds(List<int> ids, CancellationToken cancellationToken)
        {
            return await appDbContext.Directors.Where(d => ids.Distinct().Contains(d.Id)).AsNoTracking().ToDictionaryAsync(d => d.Id, d => d, cancellationToken);
        }
    }
}
