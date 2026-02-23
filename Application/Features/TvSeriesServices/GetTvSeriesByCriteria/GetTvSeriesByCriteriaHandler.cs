using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.TvSeriesServices.GetTvSeriesByCriteria
{
    public class GetTvSeriesByCriteriaHandler : IRequestHandler<GetTvSeriesByCriteriaQuery, List<TvSeriesResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ITvSeriesRepository tvSeriesRepository;
        private readonly IGenreRepository genreRepository;
        private readonly ITvSeriesSortAndFilterService SortAndFilterService;

        public GetTvSeriesByCriteriaHandler(IUnitOfWork unitOfWork, ITvSeriesSortAndFilterService sortAndFilterService, ITvSeriesRepository tvSeriesRepository, IGenreRepository genreRepository)
        {
            this.unitOfWork = unitOfWork;
            SortAndFilterService = sortAndFilterService;
            this.tvSeriesRepository = tvSeriesRepository;
            this.genreRepository = genreRepository;
        }


        public async Task<List<TvSeriesResponse>> Handle(GetTvSeriesByCriteriaQuery request, CancellationToken cancellationToken)
        {
            var query = await SortAndFilterService.Handler(request);
            var result = await tvSeriesRepository.ToListAsync(query,cancellationToken);
            var genres = await genreRepository.GetGenresDictionary();
            var Response = result.Select(tvSeries => {
                var genre = genres[tvSeries.GenreId];
                return TvSeriesMapper.ToTvSeriesResponse(tvSeries, genre);
                }).ToList();
            return Response;
        }


    }
}
