using Application.Common.Interfaces;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.MovieServices.DeleteById
{
    public class DeleteByIdHandler : IRequestHandler<DeleteByIdCommand, Unit>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMovieRepository movieRepository;
        public DeleteByIdHandler(IUnitOfWork unitOfWork, IMovieRepository movieRepository)
        {
            this.unitOfWork = unitOfWork;
            this.movieRepository = movieRepository;
        }
        public async Task<Unit> Handle(DeleteByIdCommand request, CancellationToken cancellationToken)
        {
            var movie = await movieRepository.FirstOrDefaultAsync(request.id);
            if (movie == null) throw new NotFoundException("Not found movie");
            await movieRepository.DeleteMovie(movie);
            await unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
