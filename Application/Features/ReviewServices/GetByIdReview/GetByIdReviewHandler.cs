using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using MediatR;

namespace Application.Features.ReviewServices.GetByIdReview
{
    public class GetByIdReviewHandler : IRequestHandler<GetByIdReviewQuery, ReviewResponse?>
    {
        private readonly IReviewRepository reviewRepository;
        public GetByIdReviewHandler(IReviewRepository reviewRepository)
        {
            this.reviewRepository = reviewRepository;
        }
        public async Task<ReviewResponse?> Handle(GetByIdReviewQuery query, CancellationToken cancellationToken)
        {
            var review = await reviewRepository.GetReviewByIdAsync(query.reviewId, cancellationToken);
            if (review == null)
            {
                return null;
            }
            return ReviewMapper.ToResponse(review);
        }
    }
}
