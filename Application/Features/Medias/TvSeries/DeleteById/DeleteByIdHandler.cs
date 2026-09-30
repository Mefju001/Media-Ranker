using Domain.Exceptions;
using Domain.Repository;
using MediatR;
using domain = Domain.Aggregate;

namespace Application.Features.Medias.TvSeries.DeleteById
{
    internal class DeleteByIdHandler : IRequestHandler<DeleteByIdCommand, Unit>
    {
        private readonly IMediaRepository<domain.TvSeries> mediaRepository;
        public DeleteByIdHandler(IMediaRepository<domain.TvSeries> mediaRepository)
        {

            this.mediaRepository = mediaRepository;
        }
        public async Task<Unit> Handle(DeleteByIdCommand request, CancellationToken cancellationToken)
        {
            var tvseries = await mediaRepository.GetByIdAsync(request.id, cancellationToken);
            if (tvseries == null)
            {
                throw new NotFoundException($"TvSeries with id {request.id} not found.");
            }
            mediaRepository.Remove(tvseries);
            return Unit.Value;
        }
    }
}
