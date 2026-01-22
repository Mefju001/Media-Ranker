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
        public async Task<GenreDomain?> FirstOrDefaultForNameAsync(string name)
        {
            return await _context.Genres.FirstOrDefaultAsync(g => g.name == name);
        }
        public async Task<GenreDomain> AddAsync(GenreDomain genre)
        {
            var entityEntry = await _context.Genres.AddAsync(genre);
            return entityEntry.Entity;
        }
        public GenreDomain? Get(int id)
        {
            return _context.Genres.Find(id);
        }

        public async Task<List<GenreDomain>> GetByNamesAsync(List<string> names)
        {
            return await _context.Genres.Where(g=>names.Contains(g.name)).ToListAsync();
        }
    }
}
