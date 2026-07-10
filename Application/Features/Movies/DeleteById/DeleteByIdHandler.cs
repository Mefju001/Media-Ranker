using Application.Common.Interfaces;
using Domain.Aggregate;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Movies.DeleteById
{
    // Maybe change return type to Unit, but for now it is bool to be able to return false if something went wrong with deleting the movie
    internal class DeleteByIdHandler : IRequestHandler<DeleteByIdCommand, Unit>
    {
        private readonly IMediaRepository<Movie> mediaRepository;
        public DeleteByIdHandler(IMediaRepository<Movie> mediaRepository, IMediator mediator)
        {

            this.mediaRepository = mediaRepository;
        }
        public async Task<Unit> Handle(DeleteByIdCommand request, CancellationToken cancellationToken)
        {
            var movie = await mediaRepository.GetByIdAsync(request.id, cancellationToken) ?? throw new NotFoundException($"Movie withid {request.id} does not exist.");
            mediaRepository.Remove(movie);
            return Unit.Value;
        }
    }
}
