using Microsoft.EntityFrameworkCore;
using WebApplication1.Builder.Interfaces;
using WebApplication1.Data;
using WebApplication1.DTO.Mapping;
using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.Exceptions;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class GameServices : IGameServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGameBuilder gameBuilder;

        public GameServices(IUnitOfWork unitOfWork, IGameBuilder gameBuilder)
        {
            _unitOfWork = unitOfWork;
            this.gameBuilder = gameBuilder;
        }

        public async Task<bool> Delete(int id)
        {
            var games = await _unitOfWork.Games.FirstOrDefaultAsync(x => x.Id == id);
            if (games == null)
                return false;
            _unitOfWork.Games.Delete(games);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<List<GameResponse>> GetAllAsync()
        {
            var games = await _unitOfWork.Games.AsQueryable()
                .Include(g => g.genre)
                .Include(g => g.Reviews)
                .ToListAsync();
            return games.Select(GameMapping.ToResponse).ToList();
        }

        public async Task<GameResponse?> GetById(int id)
        {
            var game = await _unitOfWork.Games.AsQueryable()
                .Include(g => g.genre)
                .Include(g => g.Reviews)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (game == null)
                throw new NotFoundException("No game found with that name");
            return GameMapping.ToResponse(game);
        }
        //editing
        public async Task<List<GameResponse>> GetGames(string? name, string? genreName)
        {
            var query = _unitOfWork.Games.AsQueryable()
                .Include(g => g.genre)
                .Include(g => g.Reviews)
                .AsQueryable();
            if (name != null)
            {
                query = query.Where(g => g.title.Contains(name));
            }
            if (genreName != null)
            {
                query = query.Where(g => g.genre.name.Contains(genreName));
            }
            /*if (directorName != null)
            {
                query = query.Where(g=>g.)
            }*/
            var games = await query.ToListAsync();
            return games.Select(GameMapping.ToResponse).ToList();
        }

        public async Task<List<GameResponse>> GetGamesByAvrRating()
        {
            var gamesAVR = await _unitOfWork.Games.AsQueryable()
                .Include(g => g.genre)
                .Include(g => g.Reviews)
                .Select(g => new
                {
                    Game = g,
                    avarage = g.Reviews.Average(r => (double?)r.Rating) ?? 0
                })
                .OrderByDescending(g => g.avarage)
                .ToListAsync();
            return gamesAVR.Select(g => GameMapping.ToResponse(g.Game)).ToList();
        }

        public async Task<List<GameResponse>> GetSortAll(string sort)
        {
            sort = sort.ToLower();
            var query = _unitOfWork.Games.AsQueryable()
                .Include(g => g.genre)
                .Include(g => g.Reviews)
                .AsQueryable();
            if (!string.IsNullOrEmpty(sort) && sort.Equals("asc"))
            {
                query = query.OrderBy(m => m.title);
            }
            if (!string.IsNullOrEmpty(sort) && sort.Equals("desc"))
            {
                query = query.OrderByDescending(m => m.title);
            }
            var games = await query.ToListAsync();
            return games.Select(GameMapping.ToResponse).ToList();
        }
        private async Task<Genre> GetOrCreateGenreAsync(GenreRequest genreRequest)
        {
            var genre = await _unitOfWork.Genres.FirstOrDefaultAsync(g => g.name == genreRequest.name);
            if (genre is not null) return genre;
            genre = new Genre { name = genreRequest.name };
            await _unitOfWork.Genres.AddAsync(genre);
            return genre;
        }
        public async Task<(int movieId, GameResponse response)> Upsert(int? gameId, GameRequest gameRequest)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var genre = await GetOrCreateGenreAsync(gameRequest.Genre);
                Game? game;
                if (gameId.HasValue)
                {
                    game = await _unitOfWork.Games.AsQueryable()
                        .Include(g => g.genre)
                        .Include(g => g.Reviews)
                        .FirstOrDefaultAsync(g => g.Id == gameId);
                    if (game != null)
                    {
                        GameMapping.UpdateEntity(game,gameRequest,genre);
                    }
                }
                else {
                    game = gameBuilder
                        .CreateNew(gameRequest.Title, gameRequest.Description, gameRequest.Platform)
                        .WithGenre(genre)
                        .WithTechnicalDetails(gameRequest.ReleaseDate, gameRequest.Language, gameRequest.Developer)
                        .Build();
                await _unitOfWork.Games.AddAsync(game);
                }
                await _unitOfWork.CompleteAsync();
                var response = GameMapping.ToResponse(game);
                await transaction.CommitAsync();
                return (game.Id, response);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
