using Application.Common.DTO.Response;
using MediatR;


namespace Application.Features.TvSeriesServices.GetTvSeriesByCriteria
{
    public class GetTvSeriesByCriteriaHandler : IRequestHandler<GetTvSeriesByCriteriaQuery, List<TvSeriesResponse>>
    {
        private readonly ITvSeriesSortAndFilterService SortAndFilterService;

        public GetTvSeriesByCriteriaHandler(ITvSeriesSortAndFilterService sortAndFilterService)
        {
            SortAndFilterService = sortAndFilterService;
        }


        public async Task<List<TvSeriesResponse>> Handle(GetTvSeriesByCriteriaQuery request, CancellationToken cancellationToken)
        {
            var Response = await SortAndFilterService.Handler(request, cancellationToken);
            return Response;
        }
    }
}
