using Application.Features.TvSeries.Common;
using MediatR;


namespace Application.Features.TvSeries.GetByCriteria
{
    internal class GetByCriteriaHandler : IRequestHandler<GetByCriteriaQuery, List<TvSeriesResponse>>
    {
        private readonly ISortAndFilterService SortAndFilterService;

        public GetByCriteriaHandler(ISortAndFilterService sortAndFilterService)
        {
            SortAndFilterService = sortAndFilterService;
        }


        public async Task<List<TvSeriesResponse>> Handle(GetByCriteriaQuery request, CancellationToken cancellationToken)
        {
            var Response = await SortAndFilterService.Handler(request, cancellationToken);
            return Response;
        }
    }
}
