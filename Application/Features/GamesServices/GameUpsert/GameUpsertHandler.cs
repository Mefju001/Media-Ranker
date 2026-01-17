using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.GamesServices.GameUpsert
{
    public class GameUpsertHandler : IRequestHandler<UpsertGameCommand, GameResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator _mediator;
        private readonly IGameBuilder gameBuilder;
        private readonly IReferenceDataService referenceDataService;

        public GameUpsertHandler(IUnitOfWork unitOfWork, IReferenceDataService referenceDataService, IGameBuilder gameBuilder, IMediator mediator)
        {
            this.unitOfWork = unitOfWork;
            this._mediator = mediator;
            this.gameBuilder = gameBuilder;
            this.referenceDataService = referenceDataService;
        }

        public async Task<GameResponse> Handle(UpsertGameCommand request, CancellationToken cancellationToken)
        {
            /*await using var transaction = await unitOfWork.BeginTransactionAsync();
            try
            {
                var genre = await referenceDataService.GetOrCreateGenreAsync(request.Genre);
                Game? game;
                if (request.id.HasValue)
                {
                    game = await unitOfWork.Games
                        .FirstOrDefaultAsync(g => g.Id == request.id);
                    if (game != null)
                    {
                        GameMapper.UpdateEntity(game, request, genre);
                    }
                }
                else
                {
                    game = gameBuilder
                        .CreateNew(request.Title, request.Description, request.Platform)
                        .WithGenre(genre)
                        .WithTechnicalDetails(request.ReleaseDate, request.Language, request.Developer)
                        .Build();
                    await unitOfWork.Games.AddAsync(game);
                }
                await unitOfWork.CompleteAsync();
                if (game is null) throw new InvalidOperationException(nameof(game));
                var response = GameMapper.ToGameResponse(game);
                await transaction.CommitAsync();
                return response;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }*/
            throw new NotImplementedException();
        }
    }
}
