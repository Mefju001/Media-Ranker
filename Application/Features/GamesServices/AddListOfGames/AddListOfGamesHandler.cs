using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.GamesServices.AddListOfGames
{
    public class AddListOfGamesHandler : IRequestHandler<AddListOfGamesCommand, List<GameResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGameBuilder gameBuilder;
        private readonly IReferenceDataService referenceDataService;
        public AddListOfGamesHandler(IReferenceDataService referenceDataService, IUnitOfWork unitOfWork, IGameBuilder builder)
        {
            this.referenceDataService = referenceDataService;
            this.gameBuilder = builder;
            this._unitOfWork = unitOfWork;
        }
        public async Task<List<GameResponse>> Handle(AddListOfGamesCommand requests, CancellationToken cancellationToken)
        {
            /*if (requests is null) throw new ArgumentNullException(nameof(requests));
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                List<Game> games = new List<Game>();
                foreach (var game in requests.requests)
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
            }*/
            throw new NotImplementedException();
        }
    }
}
