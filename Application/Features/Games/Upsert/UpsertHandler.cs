using Application.Common.Interfaces;
using Application.Features.Common.Notification;
using Application.Features.Games.Command;
using Application.Features.Games.Common;
using Application.Features.Genres.GenreManager;
using Domain.Aggregate;
using Domain.Exceptions;
using Domain.Value_Object;
using MediatR;

namespace Application.Features.Games.Upsert
{
    internal class UpsertHandler : IRequestHandler<UpsertCommand, GameResponse>
    {
        private readonly IMediaRepository<Game> mediaRepository;
        private readonly IMediator mediator;
        private readonly IGenreManager genreHelperService;

        public UpsertHandler(IGenreManager genreHelperService, IMediator mediator, IMediaRepository<Game> mediaRepository)
        {
            this.mediaRepository = mediaRepository;
            this.mediator = mediator;
            this.genreHelperService = genreHelperService;
        }

        public async Task<GameResponse> Handle(UpsertCommand request, CancellationToken cancellationToken)
        {
            var genre = await genreHelperService.GetOrCreateAsync(request.Genre, cancellationToken);
            var isNew = false;
            Game? game = null;
            if (request.id.HasValue)
            {
                game = await mediaRepository.GetByIdAsync(request.id.Value, cancellationToken) ?? throw new NotFoundException($"Game {request.id} not found");
            }
            if (game != null)
            {
                game.Update
                    (
                    request.Title,
                    request.Description,
                    new Language(request.Language),
                    new ReleaseDate(request.ReleaseDate!.Value),
                    genre.id,
                    request.Developer!,
                    request.Platforms
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
                    genre.id, request.Developer!,
                    request.Platforms);
                game = await mediaRepository.AddAsync(game, cancellationToken);
            }
            var action = isNew ? "dodana" : "zaktualizowana";
            await mediator.Publish(new LogNotification("Information", $"Gra została {action}. ID:{game.Id}", nameof(UpsertHandler)));
            return GameMapper.ToGameResponse(game, genre);
        }
    }
}
