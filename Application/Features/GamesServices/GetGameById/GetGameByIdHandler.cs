using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Aggregate;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.GamesServices.GetGameById
{
    public class GetGameByIdHandler : IRequestHandler<GetGameByIdQuery, GameResponse?>
    {
        private readonly IMediaRepository<Game> mediaRepository;
        private readonly IGenreRepository genreRepository;
        private readonly ILogger<GetGameByIdHandler> logger;

        public GetGameByIdHandler(IMediaRepository<Game> mediaRepository, IGenreRepository genreRepository, ILogger<GetGameByIdHandler> logger)
        {
            this.mediaRepository = mediaRepository;
            this.genreRepository = genreRepository;
            this.logger = logger;
        }

        public async Task<GameResponse?> Handle(GetGameByIdQuery request, CancellationToken cancellationToken)
        {
            var game = await mediaRepository.GetByIdAsync(request.id, cancellationToken);
            if (game == null)
            {
                logger.LogWarning("Game with ID {GameId} was not found.", request.id);
                throw new NotFoundException("not found");
            }
            var genre = await genreRepository.GetByIdAsync(game.GenreId, cancellationToken);
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
