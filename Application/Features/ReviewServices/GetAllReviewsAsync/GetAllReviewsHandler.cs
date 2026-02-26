using Application.Common.Interfaces;
using Domain.Entity;
using MediatR;

namespace Application.Features.ReviewServices.GetAllReviewsAsync
{
    public class GetAllReviewsHandler : IRequestHandler<GetAllReviewsQuery, List<Review>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IReviewRepository reviewRepository;
        public GetAllReviewsHandler(IUnitOfWork unitOfWork, IReviewRepository reviewRepository)
        {
            this.unitOfWork = unitOfWork;
            this.reviewRepository = reviewRepository;
        }
        public async Task<List<Review>> Handle(GetAllReviewsQuery request, CancellationToken cancellationToken)
        {
            var reviews = await reviewRepository.GetAllReviewsAsync(cancellationToken);
            return reviews;
        }
    }
}
