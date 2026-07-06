using Application.Common.Interfaces;
using Application.Features.Games.Command;
using Application.Features.Games.Common;
using Application.Features.Genres.GenreManager;
using Domain.Aggregate;
using Domain.Exceptions;
using Domain.Extensions;
using Domain.Value_Object;
using MediatR;

namespace Application.Features.Games.Upsert
{
    internal class UpsertHandler : IRequestHandler<UpsertCommand, GameResponse>
    {
        private readonly IMediaRepository<Game> mediaRepository;
        private readonly IGenreManager genreHelperService;

        public UpsertHandler(IGenreManager genreHelperService, IMediaRepository<Game> mediaRepository)
        {
            this.mediaRepository = mediaRepository;
            this.genreHelperService = genreHelperService;
        }

        public async Task<GameResponse> Handle(UpsertCommand request, CancellationToken cancellationToken)
        {
            var genre = await genreHelperService.GetOrCreateAsync(request.Genre, cancellationToken);
            Game? game = null;
            if (request.Id.HasValue)
            {
                game = await mediaRepository.GetByIdAsync(request.Id.Value, cancellationToken) ?? throw new NotFoundException($"Game {request.Id} not found");
            }
            if (game != null)
            {
                game.Update
                    (
                    request.Title,
                    request.Description,
                    request.Language,
                    new ReleaseDate(request.ReleaseDate!.Value),
                    genre.id,
                    new GameDetails(request.Developer, request.Engine),
                    request.PegiRating,
                    EPlatformExtensions.ToEnum(request.Platforms),
                    EGameStatusExtensions.ToEnum(request.GameStatus),
                    request.SupportsCrossPlay
                    );
            }
            else
            {
                game = Game.Create(
                    request.Title,
                    request.Description,
                    request.Language,
                    new ReleaseDate(request.ReleaseDate!.Value),
                    genre.id,
                    new GameDetails(request.Developer, request.Engine),
                    request.PegiRating,
                    EPlatformExtensions.ToEnum(request.Platforms),
                    EGameStatusExtensions.ToEnum(request.GameStatus),
                    request.SupportsCrossPlay);
                game = await mediaRepository.AddAsync(game, cancellationToken);
            }
            return GameMapper.ToGameResponse(game, genre);
        }
    }
}
