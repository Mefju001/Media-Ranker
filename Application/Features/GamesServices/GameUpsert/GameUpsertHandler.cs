using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Application.Notification;
using Domain.Aggregate;
using Domain.Value_Object;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.GamesServices.GameUpsert
{
    public class GameUpsertHandler : IRequestHandler<UpsertGameCommand, GameResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediaRepository mediaRepository;
        private readonly IMediator mediator;
        private readonly IReferenceDataService referenceDataService;
        private readonly ILogger<GameUpsertHandler> logger;

        public GameUpsertHandler(IUnitOfWork unitOfWork, IReferenceDataService referenceDataService, IMediator mediator, IMediaRepository mediaRepository, ILogger<GameUpsertHandler> logger)
        {
            this.mediaRepository = mediaRepository;
            this.unitOfWork = unitOfWork;
            this.mediator = mediator;
            this.referenceDataService = referenceDataService;
            this.logger = logger;
        }

        public async Task<GameResponse> Handle(UpsertGameCommand request, CancellationToken cancellationToken)
        {
            var genre = await referenceDataService.GetOrCreateGenreAsync(request.Genre, cancellationToken);
            Game? game = null;
            if (request.id.HasValue)
            {
                game = await mediaRepository.GetByIdAsync<Game>(request.id.Value, cancellationToken);
            }
            if (game != null)
            {
                game.Update
                    (
                    request.Title,
                    request.Description,
                    new Language(request.Language),
                    new ReleaseDate(request.ReleaseDate!.Value),
                    genre.Id,
                    request.Developer!,
                    request.Platform
                    );
                logger.LogInformation("Updating game with id {GameId}", game.Id);
            }
            else
            {
                game = Game.Create(
                    request.Title,
                    request.Description,
                    new Language(request.Language),
                    new ReleaseDate(request.ReleaseDate!.Value),
                    genre.Id, request.Developer!,
                    request.Platform);
                game = await mediaRepository.AddAsync<Game>(game, cancellationToken);
                logger.LogInformation("Creating new game with title {GameTitle}", game.Title);
            }
            await unitOfWork.CompleteAsync(cancellationToken);
            if (game is null) throw new InvalidOperationException(nameof(game));
            var response = GameMapper.ToGameResponse(game, genre);
            logger.LogInformation("Game with id {GameId} has been upserted successfully", game.Id);
            await mediator.Publish(new LogNotification("Information", "Nowa gra została dodana.", nameof(GameUpsertHandler)));
            return response;
        }
    }
}
