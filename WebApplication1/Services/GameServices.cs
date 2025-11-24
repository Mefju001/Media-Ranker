using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Builder.Interfaces;
using WebApplication1.Data;
using WebApplication1.DTO.Mapper;
using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.Exceptions;
using WebApplication1.Models;
using WebApplication1.QueryHandler.Query;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class GameServices: IGameServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGameBuilder gameBuilder;
        private readonly IMediator mediator;
        private readonly IReferenceDataService referenceDataService;
        public GameServices(IUnitOfWork unitOfWork, IGameBuilder gameBuilder, IMediator mediator, IReferenceDataService referenceDataService)
        {
            _unitOfWork = unitOfWork;
            this.gameBuilder = gameBuilder;
            this.mediator = mediator;
            this.referenceDataService = referenceDataService;
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
            return games.Select(GameMapper.ToGameResponse).ToList();
        }

        public async Task<GameResponse?> GetById(int id)
        {
            var game = await _unitOfWork.Games.AsQueryable()
                .Include(g => g.genre)
                .Include(g => g.Reviews)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (game == null)
                throw new NotFoundException("No game found with that name");
            return GameMapper.ToGameResponse(game);
        }
        public async Task<List<GameResponse>> GetGamesByCriteriaAsync(GameQuery gameQuery)
        {
            var games = await mediator.Send(gameQuery);
            return games;
        }
        public async Task<GameResponse> Upsert(int? gameId, GameRequest gameRequest)
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
                        GameMapper.UpdateEntity(game, gameRequest, genre);
                    }
                }
                else
                {
                    game = gameBuilder
                        .CreateNew(gameRequest.Title, gameRequest.Description, gameRequest.Platform)
                        .WithGenre(genre)
                        .WithTechnicalDetails(gameRequest.ReleaseDate, gameRequest.Language, gameRequest.Developer)
                        .Build();
                    await _unitOfWork.Games.AddAsync(game);
                }
                await _unitOfWork.CompleteAsync();
                if (game is null) throw new ArgumentNullException(nameof(game));
                var response = GameMapper.ToGameResponse(game);
                await transaction.CommitAsync();
                return response;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
