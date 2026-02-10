using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.GamesServices.GetGamesByCriteria
{
    public class GetGamesByCriteriaHandler : IRequestHandler<GetGamesByCriteriaQuery, List<GameResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IGameSortAndFilterService SortAndFilterService;

        public GetGamesByCriteriaHandler(IUnitOfWork unitOfWork, IGameSortAndFilterService sortAndFilterService)
        {
            this.unitOfWork = unitOfWork;
            SortAndFilterService = sortAndFilterService;
        }


        public async Task<List<GameResponse>> Handle(GetGamesByCriteriaQuery request, CancellationToken cancellationToken)
        {
            var query = await SortAndFilterService.ApplyFiltersAsync(request);
            query = await SortAndFilterService.ApplySorting(query, request);
            var games = await query.ToListAsync(cancellationToken);
            var genreDictionary = await unitOfWork.GenreRepository.GetGenresDictionary();
            var Response = games.Select(m =>{
                genreDictionary.TryGetValue(m.GenreId, out var genreDomain);
                return GameMapper.ToGameResponse(m, genreDomain!);
            }).ToList();
            return Response;
        }
    }
}
