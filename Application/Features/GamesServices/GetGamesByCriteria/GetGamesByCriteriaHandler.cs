using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using MediatR;


namespace Application.Features.GamesServices.GetGamesByCriteria
{
    public class GetGamesByCriteriaHandler : IRequestHandler<GetGamesByCriteriaQuery, List<GameResponse>>
    {
        private readonly IGameSortAndFilterService SortAndFilterService;
        private readonly IMediaRepository mediaRepository;
        private readonly IGenreRepository genreRepository;

        public GetGamesByCriteriaHandler(IGameSortAndFilterService sortAndFilterService, IMediaRepository mediaRepository, IGenreRepository genreRepository)
        {
            SortAndFilterService = sortAndFilterService;
            this.mediaRepository = mediaRepository;
            this.genreRepository = genreRepository;
        }


        public async Task<List<GameResponse>> Handle(GetGamesByCriteriaQuery request, CancellationToken cancellationToken)
        {
            var query = SortAndFilterService.GetGamesByCriteriaAsync(request);
            var games = await mediaRepository.FromAsQueryableToList(query, cancellationToken);
            var genreDictionary = await genreRepository.GetGenresDictionary(cancellationToken);
            var Response = games.Select(m =>
            {
                genreDictionary.TryGetValue(m.GenreId, out var genreDomain);
                return GameMapper.ToGameResponse(m, genreDomain!);
            }).ToList();
            return Response;
        }
    }
}
