using MediatR;
using WebApplication1.Application.Common.DTO.Request;
using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Application.Common.Interfaces;
using WebApplication1.Application.Mapper;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Interfaces;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class GameServices : IGameServices
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
        public async Task<List<GameResponse>> AddListOfGames(List<GameRequest> gamesRequest)
        {
            if (gamesRequest is null) throw new ArgumentNullException(nameof(gamesRequest));
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                List<Game> games = new List<Game>();
                foreach (var game in gamesRequest)
                {
                    var genre = await referenceDataService.GetOrCreateGenreAsync(game.Genre);
                    var gameBuild = gameBuilder
                        .CreateNew(game.Title, game.Description, game.Platform)
                        .WithTechnicalDetails
                        (game.ReleaseDate,
                         game.Language,
                         game.Developer)
                        .WithGenre(genre)
                        .Build();
                    games.Add(gameBuild);
                }
                await _unitOfWork.Games.AddRangeAsync(games);
                await _unitOfWork.CompleteAsync();
                var listOfResponses = games.Select(GameMapper.ToGameResponse).ToList();
                await transaction.CommitAsync();
                return listOfResponses;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
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
            var games = await _unitOfWork.Games.GetAllAsync();
            return games.Select(GameMapper.ToGameResponse).ToList();
        }

        public async Task<GameResponse?> GetById(int id)
        {
            var game = await _unitOfWork.Games.FirstOrDefaultAsync(x => x.Id == id);
            if (game is null)
                return null;
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
                    game = await _unitOfWork.Games
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
                if (game is null) throw new InvalidOperationException(nameof(game));
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
