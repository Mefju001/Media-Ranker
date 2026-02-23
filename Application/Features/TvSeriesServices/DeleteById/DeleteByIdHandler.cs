using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.TvSeriesServices.DeleteById
{
    public class DeleteByIdHandler : IRequestHandler<DeleteByIdCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ITvSeriesRepository tvSeriesRepository;
        public DeleteByIdHandler(IUnitOfWork unitOfWork, ITvSeriesRepository tvSeriesRepository)
        {
            this.unitOfWork = unitOfWork;
            this.tvSeriesRepository = tvSeriesRepository;
        }
        public async Task<bool> Handle(DeleteByIdCommand request, CancellationToken cancellationToken)
        {
            var tvSeries = await tvSeriesRepository.GetTvSeriesById(request.id);
            if (tvSeries is null)
                return false;
            await tvSeriesRepository.Delete(tvSeries);
            await unitOfWork.CompleteAsync();
            return true;
        }
    }
}
