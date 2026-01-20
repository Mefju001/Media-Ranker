using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Entity;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.GamesServices.AddListOfGames
{
    public class AddListOfGamesHandler : IRequestHandler<AddListOfGamesCommand, List<GameResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReferenceDataService referenceDataService;
        public AddListOfGamesHandler(IReferenceDataService referenceDataService, IUnitOfWork unitOfWork)
        {
            this.referenceDataService = referenceDataService;
            this._unitOfWork = unitOfWork;
        }
        public async Task<List<GameResponse>> Handle(AddListOfGamesCommand requests, CancellationToken cancellationToken)
        {
            if (requests is null) throw new ArgumentNullException(nameof(requests));
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                List<GameDomain> games = new List<GameDomain>();
                foreach (var game in requests.requests)
                {
                    var genre = await referenceDataService.GetOrCreateGenreAsync(game.Genre);
                    var gameBuild = GameDomain.Create(
                        game.Title,
                        game.Description,
                        game.Language,
                        game.ReleaseDate.GetValueOrDefault(),
                        genre,
                        game.Developer!,
                        game.Platform);
                    games.Add(gameBuild);
                }
                await _unitOfWork.GameRepository.AddListOfGames(games);
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
    }
}
