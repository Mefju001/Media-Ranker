using Application.Features.Movies.Common;
using MediatR;


namespace Application.Features.Movies.GetByCriteria
{
    internal class GetByCriteriaHandler : IRequestHandler<GetByCriteriaQuery, List<MovieResponse>>
    {
        private readonly ISortAndFilterService SortAndFilterService;

        public GetByCriteriaHandler(ISortAndFilterService sortAndFilterService)
        {
            SortAndFilterService = sortAndFilterService;
        }

        public async Task<List<MovieResponse>> Handle(GetByCriteriaQuery request, CancellationToken cancellationToken)
        {
            var responses = await SortAndFilterService.Handler(request, cancellationToken);

            return responses;
        }
    }
}
