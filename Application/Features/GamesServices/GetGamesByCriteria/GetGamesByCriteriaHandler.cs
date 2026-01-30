using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.GamesServices.GetGamesByCriteria
{
    public class GetGamesByCriteriaHandler : IRequestHandler<GetGamesByCriteriaQuery, List<GameResponse>>
    {
        private readonly IUnitOfWork context;

        public GetGamesByCriteriaHandler(IUnitOfWork unitOfWork)
        {
            this.context = unitOfWork;
        }


        public async Task<List<GameResponse>> Handle(GetGamesByCriteriaQuery request, CancellationToken cancellationToken)
        {
            var query = await context.GameRepository.AsQueryable();
            query = SortAndFilterService.ApplyFilters(query, request);
            query = SortAndFilterService.ApplySorting(query, request);
            var Response = await query.Select(m => GameMapper.ToGameResponse(m)).ToListAsync(cancellationToken);
            return Response;
        }
    }
}
