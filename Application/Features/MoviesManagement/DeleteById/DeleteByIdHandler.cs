using MediatR;
using WebApplication1.Application.Common.Interfaces;

namespace WebApplication1.Application.Features.Movies.DeleteById
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
            var movie = await unitOfWork.Movies.FirstOrDefaultAsync(x => x.Id == request.id);
            if (movie is null)
                return false;
            unitOfWork.Movies.Delete(movie);
            await unitOfWork.CompleteAsync();
            return true;
        }
    }
}
