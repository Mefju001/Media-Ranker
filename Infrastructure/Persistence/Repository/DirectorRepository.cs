using Application.Common.Interfaces;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repository
{
    public class DirectorRepository : IDirectorRepository
    {
        private readonly AppDbContext context;
        public DirectorRepository(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<Director?> FirstOrDefaultForNameAndSurnameAsync(string name, string surname)
        {
            return await context.Directors.FirstOrDefaultAsync(d => d.name == name&&d.surname == surname);
        }
        public async Task<Director> AddAsync(Director directorDomain)
        {
            var director = await context.Directors.AddAsync(directorDomain);
            return director.Entity;
        }
        public async Task<Director?> Get(int id)
        {
            return await context.Directors.FindAsync(id);
        }

        public async Task<List<Director>> findByNameAndSurname(List<string>names,List<string>surnames)
        {
            return await context.Directors.Where(d=>names.Contains(d.name)&&surnames.Contains(d.surname)).ToListAsync();
        }

        public Task<List<Director>> findByNames(List<(string, string)> names)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Director> GetAllQueryable()
        {
            return context.Directors.AsQueryable();
        }

        public Task<Dictionary<int, Director>> GetDirectorsDictionary()
        {
            var directorsDict = context.Directors.ToDictionaryAsync(d => d.Id, d => d);
            return directorsDict;
        }
    }
}
