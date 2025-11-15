using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Builder.Interfaces;
using WebApplication1.Data;
using WebApplication1.DTO.Mapping;
using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.DTO.Validator;
using WebApplication1.Exceptions;
using WebApplication1.Extensions;
using WebApplication1.Models;
using WebApplication1.QueryHandler;
using WebApplication1.QueryHandler.Query;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class GameServices(IReferenceDataService reference, IUnitOfWork unitOfWork, IGameBuilder gameBuilder, QueryServices<Game> handler, IMediator mediator) : IGameServices
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IGameBuilder gameBuilder = gameBuilder;
        private readonly QueryServices<Game> handler = handler;
        private readonly IMediator mediator = mediator;
        private readonly IReferenceDataService referenceDataService = reference;

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
            return games.Select(GameMapping.ToGameResponse).ToList();
        }

        public async Task<GameResponse?> GetById(int id)
        {
            var game = await _unitOfWork.Games.AsQueryable()
                .Include(g => g.genre)
                .Include(g => g.Reviews)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (game == null)
                throw new NotFoundException("No game found with that name");
            return GameMapping.ToGameResponse(game);
        }
        public async Task<List<GameResponse>> GetGamesByCriteriaAsync(GameQuery gameQuery)
        {
            var query = _unitOfWork.Games.AsQueryable();
            var games = await mediator.Send(gameQuery);
            return games;
        }
        public async Task<(int movieId, GameResponse response)> Upsert(int? gameId, GameRequest gameRequest)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var genre = await referenceDataService.GetOrCreateGenreAsync(gameRequest.Genre);
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
                if(game is null)throw new ArgumentNullException(nameof(game));
                var response = GameMapping.ToGameResponse(game);
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
