using Application.Common.Interfaces;
using domain = Domain.Aggregate;
using Domain.Exceptions;
using Domain.Value_Object;
using MediatR;
using Application.Features.TvSeries.Common;
using Application.Features.Genres.GenreManager;
using Application.Features.Common.Notification;

namespace Application.Features.TvSeries.Upsert
{
    internal class UpsertHandler : IRequestHandler<UpsertCommand, TvSeriesResponse>
    {
        private readonly IMediator mediator;
        private readonly IGenreManager genreHelperService;
        private readonly IMediaRepository<domain.TvSeries> mediaRepository;

        public UpsertHandler(IGenreManager genreHelperService, IMediator mediator, IMediaRepository<domain.TvSeries> mediaRepository)
        {

            this.mediator = mediator;
            this.genreHelperService = genreHelperService;
            this.mediaRepository = mediaRepository;
        }

        public async Task<TvSeriesResponse> Handle(UpsertCommand request, CancellationToken cancellationToken)
        {
            var genre = await genreHelperService.GetOrCreateAsync(request.genre, cancellationToken);
            var isNew = false;
            domain.TvSeries? tvSeries = null;
            if (request.id is not null)
            {
                tvSeries = await mediaRepository.GetByIdAsync(request.id.Value, cancellationToken) ?? throw new NotFoundException($"TvSeries {request.id} not found");
            }
            if (tvSeries is not null)
            {
                tvSeries.Update(
                    request.title,
                    request.description,
                    new Language(request.Language),
                    new ReleaseDate(request.ReleaseDate),
                    genre.id,
                    request.Seasons,
                    request.Episodes,
                    request.Network,
                    request.Status
                    );
            }
            else
            {
                isNew = true;
                tvSeries = domain.TvSeries.Create(
                        request.title,
                        request.description,
                        new Language(request.Language),
                        new ReleaseDate(request.ReleaseDate),
                        genre.id,
                        request.Seasons,
                        request.Episodes,
                        request.Network,
                        request.Status);
                tvSeries = await mediaRepository.AddAsync(tvSeries, cancellationToken);
            }
            var action = isNew ? "dodana" : "zaktualizowany";
            await mediator.Publish(new LogNotification("Information", $"Nowy serial został {action}.", nameof(UpsertHandler)));
            return TvSeriesMapper.ToTvSeriesResponse(tvSeries, genre);
        }
    }
}
