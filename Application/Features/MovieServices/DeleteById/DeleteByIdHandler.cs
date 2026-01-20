using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.MovieServices.DeleteById
{
    public class DeleteByIdHandler : IRequestHandler<DeleteByIdCommand, Unit>
    {
        private readonly IUnitOfWork unitOfWork;
        public DeleteByIdHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(DeleteByIdCommand request, CancellationToken cancellationToken)
        {
            await unitOfWork.MovieRepository.DeleteMovie(request.id);
            await unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
