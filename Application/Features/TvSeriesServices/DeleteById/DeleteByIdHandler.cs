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
            TvSeriesDomain tvSeries = null;//await unitOfWork.TvSeries.FirstOrDefaultAsync(x => x.Id == request.id);
            if (tvSeries is null)
                return false;
            //unitOfWork.TvSeries.Delete(tvSeries);
            await unitOfWork.CompleteAsync();
            return true;
        }
    }
}
