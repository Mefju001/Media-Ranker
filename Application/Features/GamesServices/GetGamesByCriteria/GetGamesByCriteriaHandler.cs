using Application.Common.DTO.Response;
using MediatR;


namespace Application.Features.GamesServices.GetGamesByCriteria
{
    public class GetGamesByCriteriaHandler : IRequestHandler<GetGamesByCriteriaQuery, List<GameResponse>>
    {
        private readonly IGameSortAndFilterService SortAndFilterService;

        public GetGamesByCriteriaHandler(IGameSortAndFilterService sortAndFilterService)
        {
            SortAndFilterService = sortAndFilterService;
        }

        public async Task<List<GameResponse>> Handle(GetGamesByCriteriaQuery request, CancellationToken cancellationToken)
        {
            var Response = await SortAndFilterService.GetGamesByCriteriaAsync(request, cancellationToken);
            return Response;
        }

    }
}
