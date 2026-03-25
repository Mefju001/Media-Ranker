using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using MediatR;

namespace Application.Features.ReviewServices.GetByIdReview
{
    public class GetByIdReviewHandler : IRequestHandler<GetByIdReviewQuery, ReviewResponse?>
    {
        private readonly IReviewRepository reviewRepository;
        private readonly IUserRepository userRepository;
        public GetByIdReviewHandler(IReviewRepository reviewRepository, IUserRepository userRepository)
        {
            this.reviewRepository = reviewRepository;
            this.userRepository = userRepository;
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
