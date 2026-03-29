using Application.Common.Interfaces;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.ReviewServices.DeleteReviewAsync
{
    public class DeleteReviewHandler : IRequestHandler<DeleteReviewCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IReviewRepository reviewRepository;
        private readonly IMediaRepository mediaRepository;
        private readonly ILogger<DeleteReviewHandler> logger;
        public DeleteReviewHandler(IUnitOfWork unitOfWork, IMediaRepository mediaRepository, IReviewRepository reviewRepository, ILogger<DeleteReviewHandler> logger)
        {
            this.unitOfWork = unitOfWork;
            this.reviewRepository = reviewRepository;
            this.mediaRepository = mediaRepository;
            this.logger = logger;
        }

        public async Task<bool> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var media = await mediaRepository.GetByIdAsync(request.mediaId, cancellationToken);
            if (media is null)
                throw new NotFoundException("There is no media with that id");
            media.DeleteReview(request.reviewId);
            await unitOfWork.CompleteAsync(cancellationToken);
            logger.LogInformation("Review with id {id} deleted successfully.", request.reviewId);
            return true;
        }
    }
}
