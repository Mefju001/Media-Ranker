using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Application.Notification;
using Domain.Aggregate;
using Domain.Exceptions;
using Domain.Value_Object;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.GamesServices.AddListOfGames
{
    public class AddListOfGamesHandler : IRequestHandler<AddListOfGamesCommand, List<GameResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IReferenceDataService referenceDataService;
        private readonly IMediaRepository mediaRepository;
        private readonly ILogger<AddListOfGamesHandler> logger;
        private readonly IMediator mediator;
        public AddListOfGamesHandler(IMediator mediator, IReferenceDataService referenceDataService, IUnitOfWork unitOfWork, IMediaRepository mediaRepository, ILogger<AddListOfGamesHandler> logger)
        {
            this.mediator = mediator;
            this.referenceDataService = referenceDataService;
            this.unitOfWork = unitOfWork;
            this.mediaRepository = mediaRepository;
            this.logger = logger;
        }
        public async Task<List<GameResponse>> Handle(AddListOfGamesCommand requests, CancellationToken cancellationToken)
        {
            if (requests.games.Count > 500)
                throw new BadRequestException("The package is too large. Maximum 500 games at a time.");
            logger.LogInformation("Received request to add a list of games. Games count: {GamesCount}", requests.games.Count);
            var names = requests.games.Select(g => g.Genre.name).Distinct().ToList();
            var genresMap = await referenceDataService.EnsureGenresExistAsync(names, cancellationToken);
            var games = requests.games.Select(gameReq =>
            {
                var genre = genresMap[gameReq.Genre.name];
                return Game.Create(
                        gameReq.Title,
                        gameReq.Description,
                        new Language(gameReq.Language),
                        new ReleaseDate(gameReq.ReleaseDate ?? DateTime.UtcNow),
                        genre.Id,
                        gameReq.Developer!,
                        gameReq.Platform);
            }).ToList();
            await mediaRepository.AddRangeAsync(games, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            logger.LogInformation("Added list of games to the repository. Games count: {GamesCount}", games.Count);
            await mediator.Publish(new LogNotification("Information", "Nowa lista gier została dodana.", nameof(AddListOfGamesHandler)));
            return games.Select(g =>
            {
                var genre = genresMap.Values.First(ge => ge.Id == g.GenreId);
                return GameMapper.ToGameResponse(g, genre);
            }).ToList();
        }
    }
}
