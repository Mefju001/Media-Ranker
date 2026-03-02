using Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.GamesServices.DeleteById
{
    public class DeleteByIdHandler : IRequestHandler<DeleteByIdCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IGameRepository gameRepository;
        private readonly ILogger<DeleteByIdHandler> logger;
        public DeleteByIdHandler(IUnitOfWork unitOfWork, IGameRepository gameRepository, ILogger<DeleteByIdHandler>logger)
        {
            this.unitOfWork = unitOfWork;
            this.gameRepository = gameRepository;
            this.logger = logger;
        }
        public async Task<bool> Handle(DeleteByIdCommand request, CancellationToken cancellationToken)
        {
            var game = await gameRepository.GetGameDomainAsync(request.id, cancellationToken);
            if (game is null)
            {
                logger.LogWarning("Game with id {id} not found for deletion.", request.id);
                return false;
            }
            try
            {
                await gameRepository.DeleteGame(game);
                await unitOfWork.CompleteAsync();
                logger.LogInformation("Game with id {id} successfully deleted.", request.id);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while deleting game with id {id}.", request.id);
                throw;
            }
        }
    }
}
