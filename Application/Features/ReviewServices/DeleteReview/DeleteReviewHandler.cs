using Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.ReviewServices.DeleteReviewAsync
{
    public class DeleteReviewHandler : IRequestHandler<DeleteReviewCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IReviewRepository reviewRepository;
        private readonly ILogger<DeleteReviewHandler> logger;
        public DeleteReviewHandler(IUnitOfWork unitOfWork, IReviewRepository reviewRepository, ILogger<DeleteReviewHandler> logger)
        {
            this.unitOfWork = unitOfWork;
            this.reviewRepository = reviewRepository;
            this.logger = logger;
        }

        public async Task<bool> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            await reviewRepository.DeleteAsync(request.reviewId, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            logger.LogInformation("Review with id {id} deleted successfully.", request.reviewId);
            return true;
        }
    }
}
