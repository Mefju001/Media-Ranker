using Application.Common.Interfaces;
using Application.Features.GamesServices.GameUpsert;
using Application.Notification;
using Domain.Aggregate;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.TvSeriesServices.DeleteById
{
    public class DeleteByIdHandler : IRequestHandler<DeleteByIdCommand, Unit>
    {
        private readonly IMediaRepository<TvSeries> mediaRepository;
        private readonly IMediator mediator;
        public DeleteByIdHandler(IMediaRepository<TvSeries> mediaRepository, IMediator mediator)
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
            await mediator.Publish(new LogNotification("Information", $"Usuwanie serialu o id: {request.id}", nameof(GameUpsertHandler)));
            return Unit.Value;
        }
    }
}
