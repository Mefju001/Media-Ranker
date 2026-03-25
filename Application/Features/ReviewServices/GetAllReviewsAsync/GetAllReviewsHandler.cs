using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using MediatR;

namespace Application.Features.ReviewServices.GetAllReviewsAsync
{
    public class GetAllReviewsHandler : IRequestHandler<GetAllReviewsQuery, List<ReviewResponse>>
    {
        private readonly IReviewRepository reviewRepository;
        private readonly IUserRepository userRepository;
        public GetAllReviewsHandler(IReviewRepository reviewRepository, IUserRepository userRepository)
        {
            this.reviewRepository = reviewRepository;
            this.userRepository = userRepository;
        }
        public async Task<List<ReviewResponse>> Handle(GetAllReviewsQuery request, CancellationToken cancellationToken)
        {
            var reviews = await reviewRepository.GetAllReviewsAsync(cancellationToken);
            return reviews.Select(r => ReviewMapper.ToResponse(r)).ToList();
        }
    }
}
