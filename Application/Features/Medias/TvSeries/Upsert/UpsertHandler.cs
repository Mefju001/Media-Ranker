using Application.Features.Genres.GenreManager;
using Application.Features.Medias.TvSeries.Common;
using Domain.Exceptions;
using Domain.Extensions;
using Domain.Repository;
using Domain.Value_Object;
using MediatR;
using domain = Domain.Aggregate;

namespace Application.Features.Medias.TvSeries.Upsert
{
    internal class UpsertHandler : IRequestHandler<UpsertCommand, TvSeriesResponse>
    {
        private readonly IGenreManager genreHelperService;
        private readonly IMediaRepository<domain.TvSeries> mediaRepository;

        public UpsertHandler(IGenreManager genreHelperService, IMediaRepository<domain.TvSeries> mediaRepository)
        {

            this.genreHelperService = genreHelperService;
            this.mediaRepository = mediaRepository;
        }

        public async Task<TvSeriesResponse> Handle(UpsertCommand request, CancellationToken cancellationToken)
        {
            var genre = await genreHelperService.GetOrCreateAsync(request.genre, cancellationToken);
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
                    request.Language,
                    new ReleaseDate(request.ReleaseDate),
                    genre.id,
                    new SeasonDetails(request.Seasons, request.Episodes),
                    request.Network,
                    ETvSeriesStatusExtensions.ToEnum(request.Status)
                    );
            }
            else
            {
                tvSeries = domain.TvSeries.Create(
                        request.title,
                        request.description,
                        request.Language,
                        new ReleaseDate(request.ReleaseDate),
                        genre.id,
                        new SeasonDetails(request.Seasons, request.Episodes),
                        request.Network,
                        ETvSeriesStatusExtensions.ToEnum(request.Status)
                        );
                tvSeries = await mediaRepository.AddAsync(tvSeries, cancellationToken);
            }
            return TvSeriesMapper.ToTvSeriesResponse(tvSeries, genre);
        }
    }
}
