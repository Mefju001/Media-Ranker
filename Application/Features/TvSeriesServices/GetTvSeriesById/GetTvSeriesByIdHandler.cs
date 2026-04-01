using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Aggregate;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.TvSeriesServices.GetTvSeriesById
{
    public class GetTvSeriesByIdHandler : IRequestHandler<GetTvSeriesByIdQuery, TvSeriesResponse?>
    {
        private readonly IMediaRepository<TvSeries> mediaRepository;
        private readonly IGenreRepository genreRepository;
        private readonly ILogger<GetTvSeriesByIdHandler> logger;

        public GetTvSeriesByIdHandler(IMediaRepository<TvSeries> mediaRepository, IGenreRepository genreRepository, ILogger<GetTvSeriesByIdHandler> logger)
        {
            this.mediaRepository = mediaRepository;
            this.genreRepository = genreRepository;
            this.logger = logger;
        }

        public async Task<TvSeriesResponse?> Handle(GetTvSeriesByIdQuery request, CancellationToken cancellationToken)
        {
            var tvSeriesDomain = await mediaRepository.GetByIdAsync(request.id, cancellationToken);
            if (tvSeriesDomain == null)
            {
                logger.LogWarning("Tv Series with ID {TvSeriesId} was not found.", request.id);
                throw new NotFoundException("not found");
            }
            var genre = await genreRepository.GetByIdAsync(tvSeriesDomain.GenreId, cancellationToken);
            if (genre == null)
            {
                logger.LogWarning("Genre with ID {GenreId} was not found for Tv Series ID {TvSeriesId}.", tvSeriesDomain.GenreId, request.id);
                throw new NotFoundException("not found");
            }
            var tvSeriesResponse = TvSeriesMapper.ToTvSeriesResponse(tvSeriesDomain, genre);
            return tvSeriesResponse;
        }
    }
}
