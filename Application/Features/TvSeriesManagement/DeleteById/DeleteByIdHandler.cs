using MediatR;
using WebApplication1.Application.Common.Interfaces;

namespace WebApplication1.Application.Features.TvSeries.DeleteById
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
            var tvSeries = await unitOfWork.TvSeries.FirstOrDefaultAsync(x => x.Id == request.id);
            if (tvSeries is null)
                return false;
            unitOfWork.TvSeries.Delete(tvSeries);
            await unitOfWork.CompleteAsync();
            return true;
        }
    }
}
