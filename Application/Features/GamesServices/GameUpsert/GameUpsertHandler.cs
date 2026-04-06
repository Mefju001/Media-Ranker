using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Application.Notification;
using Domain.Aggregate;
using Domain.Exceptions;
using Domain.Value_Object;
using MediatR;

namespace Application.Features.GamesServices.GameUpsert
{
    public class GameUpsertHandler : IRequestHandler<UpsertGameCommand, GameResponse>
    {
        private readonly IMediaRepository<Game> mediaRepository;
        private readonly IMediator mediator;
        private readonly IGenreHelperService genreHelperService;

        public GameUpsertHandler(IGenreHelperService genreHelperService, IMediator mediator, IMediaRepository<Game> mediaRepository)
        {
            this.mediaRepository = mediaRepository;
            this.mediator = mediator;
            this.genreHelperService = genreHelperService;
        }

        public async Task<GameResponse> Handle(UpsertGameCommand request, CancellationToken cancellationToken)
        {
            var genre = await genreHelperService.GetOrCreateGenreAsync(request.Genre, cancellationToken);
            var isNew = false;
            Game? game = null;
            if (request.id.HasValue)
            {
                game = await mediaRepository.GetByIdAsync(request.id.Value, cancellationToken)?? throw new NotFoundException($"Game {request.id} not found");
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
            }
            else
            {
                isNew = true;
                game = Game.Create(
                    request.Title,
                    request.Description,
                    new Language(request.Language),
                    new ReleaseDate(request.ReleaseDate!.Value),
                    genre.Id, request.Developer!,
                    request.Platform);
                game = await mediaRepository.AddAsync(game, cancellationToken);
            }
            var action = isNew ? "dodana" : "zaktualizowana";
            await mediator.Publish(new LogNotification("Information", $"Gra została {action}. ID:{game.Id}", nameof(GameUpsertHandler)));
            return GameMapper.ToGameResponse(game, genre);
        }
    }
}
