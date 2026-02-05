using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Entity;
using MediatR;

namespace Application.Features.GamesServices.GameUpsert
{
    public class GameUpsertHandler : IRequestHandler<UpsertGameCommand, GameResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator _mediator;
        private readonly IReferenceDataService referenceDataService;

        public GameUpsertHandler(IUnitOfWork unitOfWork, IReferenceDataService referenceDataService, IMediator mediator)
        {
            this.unitOfWork = unitOfWork;
            this._mediator = mediator;
            this.referenceDataService = referenceDataService;
        }

        public async Task<GameResponse> Handle(UpsertGameCommand request, CancellationToken cancellationToken)
        {
            var genre = await referenceDataService.GetOrCreateGenreAsync(request.Genre);
            GameDomain? game;
            if (request.id.HasValue)
            {
                game = await unitOfWork.GameRepository.GetGameDomainAsync(request.id.Value, cancellationToken);
                if (game != null)
                {
                    GameDomain.Update
                        (
                        request.Title,
                        request.Description,
                        request.Language,
                        request.ReleaseDate!.Value,
                        genre.Id,
                        request.Developer!,
                        request.Platform,
                        game
                        );
                }
            }
            else
            {
                game = GameDomain.Create(
                    request.Title,
                    request.Description,
                    request.Language,
                    request.ReleaseDate!.Value,
                    genre.Id, request.Developer!,
                    request.Platform);
                await unitOfWork.GameRepository.AddGameAsync(game);
            }
            await unitOfWork.CompleteAsync();
            if (game is null) throw new InvalidOperationException(nameof(game));
            var genreDomain = await unitOfWork.GenreRepository.Get(genre.Id);
            var response = GameMapper.ToGameResponse(game,genreDomain!);
            return response;
        }
    }
}
