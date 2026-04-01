using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Aggregate;
using MediatR;


namespace Application.Features.GamesServices.GetGamesByCriteria
{
    public class GetGamesByCriteriaHandler : IRequestHandler<GetGamesByCriteriaQuery, List<GameResponse>>
    {
        private readonly IGameSortAndFilterService SortAndFilterService;
        private readonly IMediaRepository<Game> mediaRepository;
        private readonly IGenreRepository genreRepository;

        public GetGamesByCriteriaHandler(IGameSortAndFilterService sortAndFilterService, IMediaRepository<Game> mediaRepository, IGenreRepository genreRepository)
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

        /*public async Task<List<GameResponse>> Handle(MediaCriteriaQuery<Game, GameResponse> request, CancellationToken cancellationToken)
        {
            var query = GameSortFilterService.GetMediaByCriteria(request);
            var games = await mediaRepository.FromAsQueryableToList(query, cancellationToken);
            var genreDictionary = await genreRepository.GetGenresDictionary(cancellationToken);
            var Response = games.Select(m =>
            {
                genreDictionary.TryGetValue(m.GenreId, out var genreDomain);
                return GameMapper.ToGameResponse(m, genreDomain!);
            }).ToList();
            return Response;
        }*/
    }
}
