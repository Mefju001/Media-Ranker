using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.GamesServices.GetAll
{
    public class GetAllHandler : IRequestHandler<GetAllQuery, List<GameResponse>>
    {
        private readonly IGameRepository gameRepository;
        private readonly IGenreRepository genreRepository;
        public GetAllHandler(IGameRepository gameRepository, IGenreRepository genreRepository)
        {
            this.gameRepository = gameRepository;
            this.genreRepository = genreRepository;
        }
        public async Task<List<GameResponse>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            var games = await gameRepository.GetAllAsync();
            var genres = await genreRepository.GetGenresDictionary(cancellationToken);
            var gameResponses = games.Select(g =>
                GameMapper.ToGameResponse(g, genres[g.GenreId])).ToList();
            return gameResponses;
        }
    }
}
