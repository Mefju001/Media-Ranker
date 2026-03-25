using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using MediatR;


namespace Application.Features.TvSeriesServices.GetTvSeriesByCriteria
{
    public class GetTvSeriesByCriteriaHandler : IRequestHandler<GetTvSeriesByCriteriaQuery, List<TvSeriesResponse>>
    {
        private readonly ITvSeriesRepository tvSeriesRepository;
        private readonly IGenreRepository genreRepository;
        private readonly ITvSeriesSortAndFilterService SortAndFilterService;

        public GetTvSeriesByCriteriaHandler(ITvSeriesSortAndFilterService sortAndFilterService, ITvSeriesRepository tvSeriesRepository, IGenreRepository genreRepository)
        {
            SortAndFilterService = sortAndFilterService;
            this.tvSeriesRepository = tvSeriesRepository;
            this.genreRepository = genreRepository;
        }


        public async Task<List<TvSeriesResponse>> Handle(GetTvSeriesByCriteriaQuery request, CancellationToken cancellationToken)
        {
            var query = SortAndFilterService.Handler(request);
            var result = await tvSeriesRepository.ToListAsync(query, cancellationToken);
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
