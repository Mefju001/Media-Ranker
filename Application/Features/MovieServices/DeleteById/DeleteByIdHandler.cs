using Application.Common.Interfaces;
using Application.Features.GamesServices.GameUpsert;
using Application.Notification;
using Domain.Aggregate;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.MovieServices.DeleteById
{
    // Maybe change return type to Unit, but for now it is bool to be able to return false if something went wrong with deleting the movie
    public class DeleteByIdHandler : IRequestHandler<DeleteByIdCommand, bool>
    {
        private readonly IMediaRepository<Movie> mediaRepository;
        private readonly IMediator mediator;
        public DeleteByIdHandler(IMediaRepository<Movie> mediaRepository, IMediator mediator)
        {

            this.mediaRepository = mediaRepository;
            this.mediator = mediator;
        }
        public async Task<bool> Handle(DeleteByIdCommand request, CancellationToken cancellationToken)
        {
            var movie = await mediaRepository.GetByIdAsync(request.id, cancellationToken) ?? throw new NotFoundException($"Movie withid {request.id} does not exist.");
            mediaRepository.Remove(movie);
            await mediator.Publish(new LogNotification("Information", $"Usunięto film o id: {request.id}", nameof(DeleteByIdHandler)));
            return true;
        }
    }
}
