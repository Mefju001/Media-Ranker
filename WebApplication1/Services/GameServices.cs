using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTO.Mapping;
using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.Exceptions;
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

        public async Task<List<GameResponse>> GetAllAsync()
        {
            var games = await _context.Games
                .Include(g => g.genre)
                .Include(g => g.Reviews)
                .ToListAsync();
            return games.Select(GameMapping.ToResponse).ToList();
        }

        public async Task<GameResponse?> GetById(int id)
        {
            var game = await _context.Games
                .Include(g => g.genre)
                .Include(g => g.Reviews)
                .FirstOrDefaultAsync(x=>x.Id == id);
            if (game == null)
                throw new NotFoundException("No game found with that name");
            return GameMapping.ToResponse(game);
        }
        //editing
        public Task<List<GameResponse>> GetGames(string? name, string? genreName)
        {
            var query = _context.Games
                .Include(g => g.genre)
                .Include(g => g.Reviews)
                .AsQueryable();
            if (name != null)
            {
                query = query.Where(g => g.title.Contains(name));
            }
            if(genreName != null)
            {
                query = query.Where(g=>g.genre.name.Contains(genreName));
            }
            /*if (directorName != null)
            {
                query = query.Where(g=>g.)
            }*/
            return null;
        }

        public Task<List<GameResponse>> GetGamesByAvrRating()
        {
            throw new NotImplementedException();
        }

        public Task<List<GameResponse>> GetSortAll(string sort)
        {
            throw new NotImplementedException();
        }

        public Task<(int movieId, Game response)> Upsert(int? movieId, MovieRequest movie)
        {
            throw new NotImplementedException();
        }
    }
}
