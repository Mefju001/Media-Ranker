using Application.Common.Interfaces;
using Domain.Entity;
using MediatR;

namespace Application.Features.TvSeriesServices.DeleteById
{
    public class DeleteByIdHandler : IRequestHandler<DeleteByIdCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        public DeleteByIdHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(DeleteByIdCommand request, CancellationToken cancellationToken)
        {
            var tvSeries = await unitOfWork.TvSeriesRepository.GetTvSeriesById(request.id);
            if (tvSeries is null)
                return false;
            await unitOfWork.TvSeriesRepository.Delete(tvSeries);
            await unitOfWork.CompleteAsync();
            return true;
        }
    }
}
