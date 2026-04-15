using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Application.Notification;
using Domain.Aggregate;
using Domain.Exceptions;
using Domain.Value_Object;
using MediatR;

namespace Application.Features.GamesServices.AddListOfGames
{
    //maybe add better response with info about which games were added and which not, and why.
    public class AddListOfGamesHandler : IRequestHandler<AddListOfGamesCommand, List<GameResponse>>
    {
        private readonly IGenreHelperService genreHelperService;
        private readonly IMediaRepository<Game> mediaRepository;
        private readonly IMediator mediator;
        public AddListOfGamesHandler(IMediator mediator, IGenreHelperService genreHelperService, IMediaRepository<Game> mediaRepository)
        {
            this.mediator = mediator;
            this.genreHelperService = genreHelperService;
            this.mediaRepository = mediaRepository;
        }
        public async Task<List<GameResponse>> Handle(AddListOfGamesCommand requests, CancellationToken cancellationToken)
        {
            if (requests.games == null || !requests.games.Any()) return [];
            if (requests.games.Count > 500)
                throw new BadRequestException("The package is too large. Maximum 500 games at a time.");
            var names = requests.games.Select(g => g.Genre.name).Distinct().ToList();
            var genresMap = await genreHelperService.EnsureGenresExistAsync(names, cancellationToken);
            var games = requests.games.Select(gameReq =>
            {
                var genre = genresMap[gameReq.Genre.name];
                return Game.Create(
                        gameReq.Title,
                        gameReq.Description,
                        new Language(gameReq.Language),
                        new ReleaseDate(gameReq.ReleaseDate ?? DateTime.UtcNow),
                        genre.Id,
                        gameReq.Developer ?? "Unknown",
                        gameReq.Platforms);
            }).ToList();
            await mediaRepository.AddRangeAsync(games, cancellationToken);
            await mediator.Publish(new LogNotification("Information", "Nowa lista gier została dodana.", nameof(AddListOfGamesHandler)));
            var genresById = genresMap.Values.ToDictionary(
                g => g.Id);
            return games.Select(g => GameMapper.ToGameResponse(g, genresById[g.GenreId])).ToList();
        }
    }
}
