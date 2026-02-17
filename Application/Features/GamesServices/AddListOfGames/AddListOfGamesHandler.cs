using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Entity;
using Domain.Value_Object;
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
            var names = requests.games.Select(g => g.Genre.name).Distinct().ToList();
            var genresMap = await referenceDataService.EnsureGenresExistAsync(names);
            var games = requests.games.Select(gameReq =>
            {
                var genre = genresMap[gameReq.Genre.name];
                return Game.Create(
                        gameReq.Title,
                        gameReq.Description,
                        new Language(gameReq.Language),
                        new ReleaseDate(gameReq.ReleaseDate.GetValueOrDefault()),
                        genre.Id,
                        gameReq.Developer!,
                        gameReq.Platform);
            }).ToList();
            await unitOfWork.GameRepository.AddListOfGames(games, cancellationToken);
            await unitOfWork.CompleteAsync();
            return games.Select(g=> {
                var genre = genresMap.Values.First(ge => ge.Id == g.GenreId);
                return GameMapper.ToGameResponse(g, genre);
                }).ToList();
        }
    }
}
