using Application.Common.Interfaces;
using Domain.Aggregate;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Games.DeleteById
{
    internal class DeleteByIdHandler : IRequestHandler<DeleteByIdCommand, Unit>
    {
        private readonly IMediaRepository<Game> mediaRepository;
        public DeleteByIdHandler(IMediaRepository<Game> mediaRepository)
        {
            this.mediaRepository = mediaRepository;
        }
        public async Task<Unit> Handle(DeleteByIdCommand request, CancellationToken cancellationToken)
        {
            var game = await mediaRepository.GetByIdAsync(request.id, cancellationToken);
            if (game is null)
            {
                throw new NotFoundException($"The game with ID {request.id} does not exist");
            }
            mediaRepository.Remove(game);
            return Unit.Value;
        }
    }
}
