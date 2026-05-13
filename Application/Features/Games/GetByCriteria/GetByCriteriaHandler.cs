using Application.Features.Games.Command;
using MediatR;


namespace Application.Features.Games.GetByCriteria
{
    internal class GetByCriteriaHandler : IRequestHandler<GetByCriteriaQuery, List<GameResponse>>
    {
        private readonly ISortAndFilterService SortAndFilterService;

        public GetByCriteriaHandler(ISortAndFilterService sortAndFilterService)
        {
            SortAndFilterService = sortAndFilterService;
        }

        public async Task<List<GameResponse>> Handle(GetByCriteriaQuery request, CancellationToken cancellationToken)
        {
            var Response = await SortAndFilterService.GetByCriteriaAsync(request, cancellationToken);
            return Response;
        }

    }
}
