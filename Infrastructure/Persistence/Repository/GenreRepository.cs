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
    public class GenreRepository : IGenreRepository
    {
        private readonly AppDbContext _context;
        public GenreRepository(AppDbContext context)
        {
            _context = context;
        }
        public  IQueryable<GenreDomain> GetAllQueryable()
        {
            return _context.Genres.AsQueryable();
        }
        public async Task<GenreDomain?> FirstOrDefaultForNameAsync(string name)
        {
            return await _context.Genres.FirstOrDefaultAsync(g => g.name == name);
        }
        public async Task<GenreDomain> AddAsync(GenreDomain genre)
        {
            var entityEntry = await _context.Genres.AddAsync(genre);
            return entityEntry.Entity;
        }
        public async Task<GenreDomain?> Get(int id)
        {
            return await _context.Genres.FindAsync(id);
        }

        public async Task<List<GenreDomain>> GetByNamesAsync(List<string> names)
        {
            return await _context.Genres.Where(g=>names.Contains(g.name)).ToListAsync();
        }

        public async Task<List<GenreDomain>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Genres.ToListAsync(cancellationToken);
        }

        public async Task<int?> GetGenreIdByNameAsync(string name, CancellationToken cancellationToken)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(g => g.name == name, cancellationToken);
            return genre?.Id;
        }

        public async Task<Dictionary<int, GenreDomain>> GetGenresDictionary()
        {
            var genresDict = await _context.Genres.ToDictionaryAsync(g => g.Id, g => g);
            return genresDict;
        }
    }
}
