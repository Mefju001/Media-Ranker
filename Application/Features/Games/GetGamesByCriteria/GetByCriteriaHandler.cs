using Application.Features.Games.Command;
using MediatR;


namespace Application.Features.Games.GetByCriteria
{
    public class GetByCriteriaHandler : IRequestHandler<GetByCriteriaQuery, List<GameResponse>>
    {
        private readonly IGameSortAndFilterService SortAndFilterService;

        public GetByCriteriaHandler(IGameSortAndFilterService sortAndFilterService)
        {
            SortAndFilterService = sortAndFilterService;
        }

        public async Task<List<GameResponse>> Handle(GetByCriteriaQuery request, CancellationToken cancellationToken)
        {
            var Response = await SortAndFilterService.GetGamesByCriteriaAsync(request, cancellationToken);
            return Response;
        }

    }
}
