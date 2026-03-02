using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.GamesServices.GetGameById
{
    public class GetGameByIdHandler : IRequestHandler<GetGameByIdQuery, GameResponse?>
    {
        private readonly IGameRepository gameRepository;
        private readonly IGenreRepository genreRepository;
        private readonly ILogger<GetGameByIdHandler> logger;

        public GetGameByIdHandler(IGameRepository gameRepository, IGenreRepository genreRepository, ILogger<GetGameByIdHandler> logger)
        {
            this.gameRepository = gameRepository;
            this.genreRepository = genreRepository;
            this.logger = logger;
        }

        public async Task<GameResponse?> Handle(GetGameByIdQuery request, CancellationToken cancellationToken)
        {
            var game = await gameRepository.GetGameDomainAsync(request.id, cancellationToken);
            if (game == null)
            {
                logger.LogWarning("Game with ID {GameId} was not found.", request.id);
                throw new NotFoundException("not found");
            }
            var genre = await genreRepository.Get(game.GenreId);
            if (genre == null)
            {
                logger.LogWarning("Genre with ID {GenreId} was not found for Game ID {GameId}.", game.GenreId, request.id);
                throw new NotFiniteNumberException("No genre found with the specified id ");
            }
            var gameResponses = GameMapper.ToGameResponse(game, genre);
            return gameResponses;
        }
    }
}
