using Application.Common.Interfaces;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.MovieServices.DeleteById
{
    public class DeleteByIdHandler : IRequestHandler<DeleteByIdCommand, Unit>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMovieRepository movieRepository;
        private readonly ILogger<DeleteByIdHandler> logger;
        public DeleteByIdHandler(IUnitOfWork unitOfWork, IMovieRepository movieRepository, ILogger<DeleteByIdHandler>logger)
        {
            this.unitOfWork = unitOfWork;
            this.movieRepository = movieRepository;
            this.logger = logger;
        }
        public async Task<Unit> Handle(DeleteByIdCommand request, CancellationToken cancellationToken)
        {
            var movie = await movieRepository.FirstOrDefaultAsync(request.id, cancellationToken);
            if (movie == null)
            {
                logger.LogWarning("Movie with id {id} not found for deletion.", request.id);
                throw new NotFoundException($"Movie with id {request.id} not found.");
            }
            try {
                await movieRepository.DeleteMovie(movie, cancellationToken);
                await unitOfWork.CompleteAsync(cancellationToken);
                logger.LogInformation("Game with id {id} successfully deleted.", request.id);
                return Unit.Value;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while deleting movie with id {id}.", request.id);
                throw;
            }
        }
    }
}
