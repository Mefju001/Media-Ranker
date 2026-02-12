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
        private readonly ITvSeriesSortAndFilterService SortAndFilterService;

        public GetTvSeriesByCriteriaHandler(IUnitOfWork unitOfWork, ITvSeriesSortAndFilterService sortAndFilterService)
        {
            this.unitOfWork = unitOfWork;
            SortAndFilterService = sortAndFilterService;
        }


        public async Task<List<TvSeriesResponse>> Handle(GetTvSeriesByCriteriaQuery request, CancellationToken cancellationToken)
        {
            var query = await SortAndFilterService.Handler(request);
            var result = await unitOfWork.TvSeriesRepository.ToListAsync(query,cancellationToken);
            var genres = await unitOfWork.GenreRepository.GetGenresDictionary();
            var Response = result.Select(tvSeries => {
                var genre = genres[tvSeries.GenreId];
                return TvSeriesMapper.ToTvSeriesResponse(tvSeries, genre);
                }).ToList();
            return Response;
        }


    }
}
