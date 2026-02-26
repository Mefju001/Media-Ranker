using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Entity;
using Domain.Value_Object;
using MediatR;

namespace Application.Features.GamesServices.GameUpsert
{
    public class GameUpsertHandler : IRequestHandler<UpsertGameCommand, GameResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IGameRepository gameRepository;
        private readonly IGenreRepository genreRepository;
        private readonly IMediator _mediator;
        private readonly IReferenceDataService referenceDataService;

        public GameUpsertHandler(IUnitOfWork unitOfWork, IReferenceDataService referenceDataService, IMediator mediator, IGameRepository gameRepository, IGenreRepository genreRepository)
        {
            this.unitOfWork = unitOfWork;
            this._mediator = mediator;
            this.referenceDataService = referenceDataService;
        }

        public async Task<GameResponse> Handle(UpsertGameCommand request, CancellationToken cancellationToken)
        {
            var genre = await referenceDataService.GetOrCreateGenreAsync(request.Genre);
            Game? game;
            if (request.id.HasValue)
            {
                game = await gameRepository.GetGameDomainAsync(request.id.Value, cancellationToken);
                if (game != null)
                {
                    game.Update
                        (
                        request.Title,
                        request.Description,
                        new Language(request.Language),
                        new ReleaseDate(request.ReleaseDate!.Value),
                        genre.Id,
                        request.Developer!,
                        request.Platform
                        );
                }
            }
            else
            {
                game = Game.Create(
                    request.Title,
                    request.Description,
                    new Language(request.Language),
                    new ReleaseDate(request.ReleaseDate!.Value),
                    genre.Id, request.Developer!,
                    request.Platform);
                await gameRepository.AddGameAsync(game);
            }
            await unitOfWork.CompleteAsync();
            if (game is null) throw new InvalidOperationException(nameof(game));
            var genreDomain = await genreRepository.Get(genre.Id);
            var response = GameMapper.ToGameResponse(game, genreDomain!);
            return response;
        }
    }
}
