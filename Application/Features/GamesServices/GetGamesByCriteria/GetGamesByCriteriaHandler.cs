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
        private readonly IGameRepository gameRepository;
        private readonly IGenreRepository genreRepository;

        public GetGamesByCriteriaHandler(IUnitOfWork unitOfWork, IGameSortAndFilterService sortAndFilterService, IGameRepository gameRepository, IGenreRepository genreRepository)
        {
            this.unitOfWork = unitOfWork;
            SortAndFilterService = sortAndFilterService;
            this.gameRepository = gameRepository;
            this.genreRepository = genreRepository;
        }


        public async Task<List<GameResponse>> Handle(GetGamesByCriteriaQuery request, CancellationToken cancellationToken)
        {
            var query = await SortAndFilterService.GetGamesByCriteriaAsync(request);
            var games = await gameRepository.GetListFromQueryAsync(query, cancellationToken);
            var genreDictionary = await genreRepository.GetGenresDictionary();
            var Response = games.Select(m =>{
                genreDictionary.TryGetValue(m.GenreId, out var genreDomain);
                return GameMapper.ToGameResponse(m, genreDomain!);
            }).ToList();
            return Response;
        }
    }
}
