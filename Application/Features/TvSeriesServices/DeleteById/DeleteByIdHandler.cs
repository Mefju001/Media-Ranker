using Application.Common.Interfaces;
using Application.Features.GamesServices.GameUpsert;
using Application.Notification;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.TvSeriesServices.DeleteById
{
    public class DeleteByIdHandler : IRequestHandler<DeleteByIdCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ITvSeriesRepository tvSeriesRepository;
        private readonly ILogger<DeleteByIdHandler> logger;
        private readonly IMediator mediator;
        public DeleteByIdHandler(IUnitOfWork unitOfWork, ITvSeriesRepository tvSeriesRepository, ILogger<DeleteByIdHandler>logger, IMediator mediator)
        {
            this.unitOfWork = unitOfWork;
            this.tvSeriesRepository = tvSeriesRepository;
            this.logger = logger;
            this.mediator = mediator;
        }
        public async Task<bool> Handle(DeleteByIdCommand request, CancellationToken cancellationToken)
        {
            var tvseries = await tvSeriesRepository.GetTvSeriesById(request.id,cancellationToken);
            if (tvseries == null)
            {
                logger.LogWarning("TvSeries with id {id} not found.", request.id);
                throw new NotFoundException($"TvSeries with id {request.id} not found.");
            }
            tvSeriesRepository.Delete(tvseries);
            await unitOfWork.CompleteAsync(cancellationToken);
            logger.LogInformation("TvSeries with id {id} successfully deleted.", request.id);
            await mediator.Publish(new LogNotification("Information", $"Usuwanie serialu o id: {request.id}", nameof(GameUpsertHandler)));
            return true;
        }
    }
}
