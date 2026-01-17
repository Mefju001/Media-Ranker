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
            throw new NotImplementedException();
            /*var game = await unitOfWork.Games.FirstOrDefaultAsync(x => x.Id == request.id);
            if (game is null)
                return false;
            unitOfWork.Games.Delete(game);
            await unitOfWork.CompleteAsync();
            return true;*/
        }
    }
}
