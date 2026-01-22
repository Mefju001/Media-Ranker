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
        private readonly IUnitOfWork unitOfWork;
        private readonly IReferenceDataService referenceDataService;
        public AddListOfGamesHandler(IReferenceDataService referenceDataService, IUnitOfWork unitOfWork)
        {
            this.referenceDataService = referenceDataService;
            this.unitOfWork = unitOfWork;
        }
        public async Task<List<GameResponse>> Handle(AddListOfGamesCommand requests, CancellationToken cancellationToken)
        {
            var names = requests.games.Select(g=>g.Genre.name).Distinct().ToList();
            var genresMap = await referenceDataService.EnsureGenresExistAsync(names);
            var games = requests.games.Select(gameReq => {
                var genre = genresMap[gameReq.Genre.name];
                return GameDomain.Create(
                        gameReq.Title,
                        gameReq.Description,
                        gameReq.Language,
                        gameReq.ReleaseDate.GetValueOrDefault(),
                        genre,
                        gameReq.Developer!,
                        gameReq.Platform);
            }).ToList();
            await unitOfWork.GameRepository.AddListOfGames(games, cancellationToken);
            await unitOfWork.CompleteAsync();

            return games.Select(GameMapper.ToGameResponse).ToList();
        }
    }
}
