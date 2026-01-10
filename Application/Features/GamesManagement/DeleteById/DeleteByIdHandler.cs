using MediatR;
using WebApplication1.Application.Common.Interfaces;

namespace WebApplication1.Application.Features.Games.DeleteById
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
            var game = await unitOfWork.Games.FirstOrDefaultAsync(x => x.Id == request.id);
            if (game is null)
                return false;
            unitOfWork.Games.Delete(game);
            await unitOfWork.CompleteAsync();
            return true;
        }
    }
}
