using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.GamesServices.DeleteById
{
    public class DeleteByIdHandler : IRequestHandler<DeleteByIdCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IGameRepository gameRepository;
        public DeleteByIdHandler(IUnitOfWork unitOfWork, IGameRepository gameRepository)
        {
            this.unitOfWork = unitOfWork;
            this.gameRepository = gameRepository;
        }
        public async Task<bool> Handle(DeleteByIdCommand request, CancellationToken cancellationToken)
        {
            var game = await gameRepository.GetGameDomainAsync(request.id, cancellationToken);
            if (game is null)
                return false;
            await gameRepository.DeleteGame(game);
            await unitOfWork.CompleteAsync();
            return true;
        }
    }
}
