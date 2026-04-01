using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Aggregate;
using MediatR;


namespace Application.Features.TvSeriesServices.GetTvSeriesByCriteria
{
    public class GetTvSeriesByCriteriaHandler : IRequestHandler<GetTvSeriesByCriteriaQuery, List<TvSeriesResponse>>
    {
        private readonly IMediaRepository<TvSeries> mediaRepository;
        private readonly IGenreRepository genreRepository;
        private readonly ITvSeriesSortAndFilterService SortAndFilterService;

        public GetTvSeriesByCriteriaHandler(ITvSeriesSortAndFilterService sortAndFilterService, IMediaRepository<TvSeries> mediaRepository, IGenreRepository genreRepository)
        {
            SortAndFilterService = sortAndFilterService;
            this.mediaRepository = mediaRepository;
            this.genreRepository = genreRepository;
        }


        public async Task<List<TvSeriesResponse>> Handle(GetTvSeriesByCriteriaQuery request, CancellationToken cancellationToken)
        {
            var query = SortAndFilterService.Handler(request);
            var result = await mediaRepository.FromAsQueryableToList(query, cancellationToken);
            var genres = await genreRepository.GetGenresDictionary(cancellationToken);
            var Response = result.Select(tvSeries =>
            {
                genres.TryGetValue(tvSeries.GenreId, out var genre);
                return TvSeriesMapper.ToTvSeriesResponse(tvSeries, genre);
            }).ToList();
            return Response;
        }
    }
}
