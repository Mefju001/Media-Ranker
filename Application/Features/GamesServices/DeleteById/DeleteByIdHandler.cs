using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.GamesServices.DeleteById
{
    public class DeleteByIdHandler : IRequestHandler<DeleteByIdCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        public DeleteByIdHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(DeleteByIdCommand request, CancellationToken cancellationToken)
        {
            var game = await unitOfWork.GameRepository.GetGameDomainAsync(request.id,cancellationToken);
            if (game is null)
                return false;
            await unitOfWork.GameRepository.DeleteGame(game);
            await unitOfWork.CompleteAsync();
            return true;
        }
    }
}
