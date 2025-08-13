using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTO.Request;
using WebApplication1.Models;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class GameServices : IGameServices
    {
        private readonly AppDbContext _context;
        public async Task<bool> Delete(int id)
        {
            var games = _context.Games.FirstOrDefault(x=>x.Id == id);
            if(games == null)
                return false;
            _context.Games.Remove(games);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Game>> GetAllAsync()
        {
            var games = await _context.Games
                .Include(g => g.genre)
                .Include(g => g.Reviews)
                .ToListAsync();
            return games;
        }

        public Task<Game?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Game>> GetGames(string? name, string? genreId, string? directorId, int? movieid)
        {
            throw new NotImplementedException();
        }

        public Task<List<Game>> GetGamesByAvrRating()
        {
            throw new NotImplementedException();
        }

        public Task<List<Game>> GetSortAll(string sort)
        {
            throw new NotImplementedException();
        }

        public Task<(int movieId, Game response)> Upsert(int? movieId, MovieRequest movie)
        {
            throw new NotImplementedException();
        }
    }
}
