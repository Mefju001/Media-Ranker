using Application.Common.Interfaces;
using Domain.Aggregate;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.ReviewServices.DeleteReviewAsync
{
    public class DeleteReviewHandler : IRequestHandler<DeleteReviewCommand, bool>
    {
        private readonly IMediaRepository<Media> mediaRepository;
        private readonly ILogger<DeleteReviewHandler> logger;
        public DeleteReviewHandler(IMediaRepository<Media> mediaRepository, ILogger<DeleteReviewHandler> logger)
        {
            
            this.mediaRepository = mediaRepository;
            this.logger = logger;
        }

        public async Task<bool> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var media = await mediaRepository.GetByIdAsync(request.mediaId, cancellationToken);
            if (media is null)
                throw new NotFoundException("There is no media with that id");
            media.DeleteReview(request.reviewId);
            
            logger.LogInformation("Review with id {id} deleted successfully.", request.reviewId);
            return true;
        }
    }
}
