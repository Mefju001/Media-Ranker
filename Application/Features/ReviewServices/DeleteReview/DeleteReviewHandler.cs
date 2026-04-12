using Application.Common.Interfaces;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.ReviewServices.DeleteReviewAsync
{
    public class DeleteReviewHandler : IRequestHandler<DeleteReviewCommand, bool>
    {
        private readonly IMediaRepository<Media> mediaRepository;
        public DeleteReviewHandler(IMediaRepository<Media> mediaRepository)
        {
            this.mediaRepository = mediaRepository;
        }

        public async Task<bool> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var media = await mediaRepository.GetByIdAsync(request.mediaId, cancellationToken);
            if (media is null)
                throw new NotFoundException("There is no media with that id");
            media.DeleteReview(request.reviewId);
            return true;
        }
    }
}
