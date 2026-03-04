using Application.Common.Interfaces;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen;

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
            return await context.Directors.FirstOrDefaultAsync(d => d.name == name && d.surname == surname, cancellationToken);
        }
        public async Task<Director> AddAsync(Director directorDomain, CancellationToken cancellationToken)
        {
            var director = await context.Directors.AddAsync(directorDomain,cancellationToken);
            return director.Entity;
        }
        public async Task<Director?> Get(int id, CancellationToken cancellationToken)
        {
            return await context.Directors.FindAsync(id,cancellationToken);
        }

        public async Task<List<Director>> findByNameAndSurname(List<string> names, List<string> surnames, CancellationToken cancellationToken)
        {
            return await context.Directors.Where(d => names.Contains(d.name) && surnames.Contains(d.surname)).ToListAsync(cancellationToken);
        }

        public Task<List<Director>> findByNames(List<(string, string)> names, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Director> GetAllQueryable()
        {
            return context.Directors.AsQueryable();
        }

        public Task<Dictionary<int, Director>> GetDirectorsDictionary(CancellationToken cancellationToken)
        {
            var directorsDict = context.Directors.ToDictionaryAsync(d => d.Id, d => d, cancellationToken);
            return directorsDict;
        }

        public async Task<Dictionary<int,Director>> GetByIds(List<int> ids, CancellationToken cancellationToken)
        {
            return await context.Directors.Where(d => ids.Contains(d.Id)).ToDictionaryAsync(m=>m.Id,cancellationToken);
        }
    }
}
