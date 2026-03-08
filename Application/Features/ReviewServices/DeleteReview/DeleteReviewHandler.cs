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
        private readonly ILogger<DeleteReviewHandler> logger;
        public DeleteReviewHandler(IUnitOfWork unitOfWork, IReviewRepository reviewRepository, ILogger<DeleteReviewHandler>logger)
        {
            this.unitOfWork = unitOfWork;
            this.reviewRepository = reviewRepository;
            this.logger = logger;
        }

        public async Task<bool> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await reviewRepository.GetReviewByIdAsync(request.reviewId,cancellationToken);
            if (review is null)
            {
                logger.LogWarning("Review with id {id} not found for deletion.", request.reviewId);
                throw new NotFoundException($"Review with id: {request.reviewId} does not exist");
            }
            await reviewRepository.DeleteAsync(review);
            await unitOfWork.CompleteAsync(cancellationToken);
            logger.LogInformation("Review with id {id} deleted successfully.", request.reviewId);
            return true;
        }
    }
}
