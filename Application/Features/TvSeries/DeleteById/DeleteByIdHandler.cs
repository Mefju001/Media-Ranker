using Application.Common.Interfaces;
using domain = Domain.Aggregate;
using Domain.Exceptions;
using MediatR;
using Application.Features.Common.Notification;

namespace Application.Features.TvSeries.DeleteById
{
    internal class DeleteByIdHandler : IRequestHandler<DeleteByIdCommand, Unit>
    {
        private readonly IMediaRepository<domain.TvSeries> mediaRepository;
        private readonly IMediator mediator;
        public DeleteByIdHandler(IMediaRepository<domain.TvSeries> mediaRepository, IMediator mediator)
        {

            this.mediaRepository = mediaRepository;
            this.mediator = mediator;
        }
        public async Task<Unit> Handle(DeleteByIdCommand request, CancellationToken cancellationToken)
        {
            var tvseries = await mediaRepository.GetByIdAsync(request.id, cancellationToken);
            if (tvseries == null)
            {
                throw new NotFoundException($"TvSeries with id {request.id} not found.");
            }
            mediaRepository.Remove(tvseries);
            await mediator.Publish(new LogNotification("Information", $"Usunięto serial o id: {request.id}", nameof(DeleteByIdHandler)));
            return Unit.Value;
        }
    }
}
