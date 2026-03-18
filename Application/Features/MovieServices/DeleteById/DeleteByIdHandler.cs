using Application.Common.Interfaces;
using Application.Features.GamesServices.GameUpsert;
using Application.Notification;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;

namespace Application.Features.MovieServices.DeleteById
{
    public class DeleteByIdHandler : IRequestHandler<DeleteByIdCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMovieRepository movieRepository;
        private readonly ILogger<DeleteByIdHandler> logger;
        private readonly IMediator mediator;
        public DeleteByIdHandler(IUnitOfWork unitOfWork, IMovieRepository movieRepository, ILogger<DeleteByIdHandler>logger, IMediator mediator)
        {
            this.unitOfWork = unitOfWork;
            this.movieRepository = movieRepository;
            this.logger = logger;
            this.mediator = mediator;
        }
        public async Task<bool> Handle(DeleteByIdCommand request, CancellationToken cancellationToken)
        {
            var movie = await movieRepository.FirstOrDefaultAsync(request.id, cancellationToken);
            if(movie == null)
            {
                logger.LogWarning("Movie with id {id} does not exist.",request.id);
                throw new NotFoundException($"Movie withid {request.id} does not exist.");
            }
            movieRepository.DeleteMovie(movie);
            await unitOfWork.CompleteAsync(cancellationToken);
            logger.LogInformation("Game with id {id} successfully deleted.", request.id);
            await mediator.Publish(new LogNotification("Information", $"Usuwanie filmu o id: {request.id}", nameof(GameUpsertHandler)));
            return true;
        }
    }
}
