using Application.Common.Interfaces;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class DirectorRepository : IDirectorRepository
    {
        private readonly AppDbContext context;
        public DirectorRepository(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<Director?> FirstOrDefaultForNameAndSurnameAsync(string name, string surname, CancellationToken cancellationToken)
        {
            return await context.Directors.AsNoTracking().FirstOrDefaultAsync(d => d.Name == name && d.Surname == surname, cancellationToken);
        }
        public async Task<Director> AddAsync(Director directorDomain, CancellationToken cancellationToken)
        {
            var director = await context.Directors.AddAsync(directorDomain, cancellationToken);
            return director.Entity;
        }
        public async Task<Director?> Get(int id, CancellationToken cancellationToken)
        {
            return await context.Directors.AsNoTrackingWithIdentityResolution().FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }
        public async Task<List<Director>> findByNames(List<(string, string)> fullnames, CancellationToken cancellationToken)
        {
            var names = fullnames.Select(x => x.Item1).Distinct().ToList();
            var surnames = fullnames.Select(x => x.Item2).Distinct().ToList();
            var results = await context.Directors.Where(d => names.Contains(d.Name) && surnames.Contains(d.Surname)).AsNoTracking().ToListAsync(cancellationToken);
            return results;
        }

        public IQueryable<Director> GetAllQueryable()
        {
            return context.Directors.AsQueryable();
        }

        public Task<Dictionary<int, Director>> GetDirectorsDictionary(CancellationToken cancellationToken)
        {
            var directorsDict = context.Directors.AsNoTracking().ToDictionaryAsync(d => d.Id, d => d, cancellationToken);
            return directorsDict;
        }

        public async Task<Dictionary<int, Director>> GetByIds(List<int> ids, CancellationToken cancellationToken)
        {
            return await context.Directors.Where(d => ids.Distinct().Contains(d.Id)).AsNoTracking().ToDictionaryAsync(d => d.Id, d => d, cancellationToken);
        }
    }
}
