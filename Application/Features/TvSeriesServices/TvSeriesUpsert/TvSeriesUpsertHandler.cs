using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Application.Notification;
using Domain.Aggregate;
using Domain.Value_Object;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.TvSeriesServices.TvSeriesUpsert
{
    public class TvSeriesUpsertHandler : IRequestHandler<UpsertTvSeriesCommand, TvSeriesResponse>
    {
        private readonly IMediator mediator;
        private readonly IGenreHelperService genreHelperService;
        private readonly IMediaRepository<TvSeries> mediaRepository;
        private readonly ILogger<TvSeriesUpsertHandler> logger;

        public TvSeriesUpsertHandler(IGenreHelperService genreHelperService, IMediator mediator, IMediaRepository<TvSeries> mediaRepository, ILogger<TvSeriesUpsertHandler> logger)
        {
            
            this.mediator = mediator;
            this.genreHelperService = genreHelperService;
            this.mediaRepository = mediaRepository;
            this.logger = logger;
        }

        public async Task<TvSeriesResponse> Handle(UpsertTvSeriesCommand request, CancellationToken cancellationToken)
        {
            var genre = await genreHelperService.GetOrCreateGenreAsync(request.genre, cancellationToken);
            TvSeries? tvSeries = null;
            if (request.id is not null)
            {
                tvSeries = await mediaRepository.GetByIdAsync(request.id.Value, cancellationToken);

            }
            if (tvSeries is not null)
            {
                tvSeries.Update(
                    request.title,
                    request.description,
                    new Language(request.Language),
                    new ReleaseDate(request.ReleaseDate),
                    genre.Id,
                    request.Seasons,
                    request.Episodes,
                    request.Network,
                    request.Status
                    );
                logger.LogInformation("Updating TvSeries with id {TvSeriesId}", tvSeries.Id);
            }
            else
            {
                tvSeries = TvSeries.Create(
                        request.title,
                        request.description,
                        new Language(request.Language),
                        new ReleaseDate(request.ReleaseDate),
                        genre.Id,
                        request.Seasons,
                        request.Episodes,
                        request.Network,
                        request.Status);
                tvSeries = await mediaRepository.AddAsync(tvSeries, cancellationToken);
                logger.LogInformation("Creating new TvSeries with id {TvSeriesId}", tvSeries.Id);
            }
            
            if (tvSeries is null) throw new ArgumentNullException(nameof(tvSeries));
            var response = TvSeriesMapper.ToTvSeriesResponse(tvSeries, genre);
            logger.LogInformation("TvSeries with id {TvSeriesId} upserted successfully", tvSeries.Id);
            await mediator.Publish(new LogNotification("Information", "Nowy serial został dodany.", nameof(TvSeriesUpsertHandler)));
            return response;
        }
    }
}
