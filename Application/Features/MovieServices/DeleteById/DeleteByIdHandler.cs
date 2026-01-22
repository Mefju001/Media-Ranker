using Application.Common.Interfaces;
using Domain.Exceptions;
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
            var movie = await unitOfWork.MovieRepository.FirstOrDefaultAsync(request.id);
            if (movie == null) throw new NotFoundException("Not found movie");
            await unitOfWork.MovieRepository.DeleteMovie(movie);
            await unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
