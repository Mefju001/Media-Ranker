using Application.Common.Interfaces;
using Application.Features.GamesServices.GameUpsert;
using Application.Notification;
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
            var tvSeries = await tvSeriesRepository.GetTvSeriesById(request.id);
            if (tvSeries is null)
            {
                logger.LogWarning("TvSeries with id {id} not found.", request.id);
                return false;
            }
            await tvSeriesRepository.Delete(tvSeries);
            await unitOfWork.CompleteAsync(cancellationToken);
            logger.LogInformation("TvSeries with id {id} successfully deleted.", request.id);
            await mediator.Publish(new LogNotification("Information", $"Usuwanie gry o id: {request.id}", nameof(GameUpsertHandler)));
            return true;
        }
    }
}
