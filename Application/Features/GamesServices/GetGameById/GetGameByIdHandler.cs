using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.GamesServices.GetGameById
{
    public class GetGameByIdHandler : IRequestHandler<GetGameByIdQuery, GameResponse?>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IGameRepository gameRepository;
        private readonly IGenreRepository genreRepository;

        public GetGameByIdHandler(IUnitOfWork unitOfWork, IGameRepository gameRepository, IGenreRepository genreRepository)
        {
            this.unitOfWork = unitOfWork;
            this.gameRepository = gameRepository;
            this.genreRepository = genreRepository;
        }

        public async Task<GameResponse?> Handle(GetGameByIdQuery request, CancellationToken cancellationToken)
        {
            var game = await gameRepository.GetGameDomainAsync(request.id, cancellationToken);
            var genre = await genreRepository.GetGenresDictionary();
            if (game == null)
            {
                throw new NotFoundException("not found");
            }
            var gameResponses = GameMapper.ToGameResponse(game, genre[game.GenreId]);
            return gameResponses;
        }
    }
}
