using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Aggregate;
using MediatR;

namespace Application.Features.GamesServices.GetAll
{
    public class GetAllHandler : IRequestHandler<GetAllQuery, List<GameResponse>>
    {
        private readonly IMediaRepository<Game> mediaRepository;
        private readonly IGenreRepository genreRepository;
        public GetAllHandler(IMediaRepository<Game> mediaRepository, IGenreRepository genreRepository)
        {
            this.mediaRepository = mediaRepository;
            this.genreRepository = genreRepository;
        }
        public async Task<List<GameResponse>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            var games = await mediaRepository.GetAllAsync(cancellationToken);
            var genres = await genreRepository.GetGenresDictionary(cancellationToken);
            var gameResponses = games.Select(g =>
                GameMapper.ToGameResponse(g, genres[g.GenreId])).ToList();
            return gameResponses;
        }
    }
}
