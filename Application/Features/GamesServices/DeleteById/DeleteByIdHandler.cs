using Application.Common.Interfaces;
using Application.Notification;
using Domain.Aggregate;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.GamesServices.DeleteById
{
    public class DeleteByIdHandler : IRequestHandler<DeleteByIdCommand, Unit>
    {
        private readonly IMediaRepository<Game> mediaRepository;
        private readonly IMediator mediator;
        public DeleteByIdHandler(IMediaRepository<Game> mediaRepository, IMediator mediator)
        {
            this.mediaRepository = mediaRepository;
            this.mediator = mediator;
        }
        public async Task<Unit> Handle(DeleteByIdCommand request, CancellationToken cancellationToken)
        {
            var game = await mediaRepository.GetByIdAsync(request.id, cancellationToken);
            if (game is null)
            {
                throw new NotFoundException($"The game with ID {request.id} does not exist");
            }
            mediaRepository.Remove(game);
            await mediator.Publish(new LogNotification("Information", $"usunięto grę o id: {request.id}", nameof(DeleteByIdHandler)));
            return Unit.Value;
        }
    }
}
