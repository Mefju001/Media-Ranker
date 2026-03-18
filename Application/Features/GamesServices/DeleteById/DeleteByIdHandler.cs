using Application.Common.Interfaces;
using Application.Features.GamesServices.GameUpsert;
using Application.Notification;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;

namespace Application.Features.GamesServices.DeleteById
{
    public class DeleteByIdHandler : IRequestHandler<DeleteByIdCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IGameRepository gameRepository;
        private readonly ILogger<DeleteByIdHandler> logger;
        private readonly IMediator mediator;
        public DeleteByIdHandler(IUnitOfWork unitOfWork, IGameRepository gameRepository, ILogger<DeleteByIdHandler>logger, IMediator mediator)
        {
            this.unitOfWork = unitOfWork;
            this.gameRepository = gameRepository;
            this.logger = logger;
            this.mediator = mediator;
        }
        public async Task<bool> Handle(DeleteByIdCommand request, CancellationToken cancellationToken)
        {
            var game = await gameRepository.GetGameDomainAsync(request.id, cancellationToken);
            if (game is null)
            {
                logger.LogWarning("Game with id {id} not found.", request.id);
                throw new NotFoundException($"The game with ID {request.id} does not exist");
            }
            gameRepository.DeleteGame(game);
            await unitOfWork.CompleteAsync(cancellationToken);
            logger.LogInformation("Game with id {id} successfully deleted.", request.id);
            await mediator.Publish(new LogNotification("Information", $"Usuwanie gry o id: {request.id}", nameof(GameUpsertHandler)));
            return true;
        }
    }
}
