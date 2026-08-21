using Domain.Exceptions;
using Domain.Repository;
using MediatR;

namespace Application.Features.UserInteractions.Reviews.DeleteById
{
    internal class DeleteByIdHandler : IRequestHandler<DeleteByIdCommand, bool>
    {
        private readonly IMediaRepository<Media> mediaRepository;
        public DeleteByIdHandler(IMediaRepository<Media> mediaRepository)
        {
            this.mediaRepository = mediaRepository;
        }

        public async Task<bool> Handle(DeleteByIdCommand request, CancellationToken cancellationToken)
        {
            var media = await mediaRepository.GetByIdAsync(request.mediaId, cancellationToken);
            if (media is null)
                throw new NotFoundException("There is no media with that id");
            media.DeleteReview(request.reviewId);
            return true;
        }
    }
}
