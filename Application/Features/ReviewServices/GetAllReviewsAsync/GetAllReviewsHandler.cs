using Application.Common.Interfaces;
using Application.Common.DTO.Response;
using MediatR;
using Application.Mapper;

namespace Application.Features.ReviewServices.GetAllReviewsAsync
{
    public class GetAllReviewsHandler : IRequestHandler<GetAllReviewsQuery, List<ReviewResponse>>
    {
        private readonly IReviewRepository reviewRepository;
        public GetAllReviewsHandler(IReviewRepository reviewRepository)
        {
            this.reviewRepository = reviewRepository;
        }
        public async Task<List<ReviewResponse>> Handle(GetAllReviewsQuery request, CancellationToken cancellationToken)
        {
            var reviews = await reviewRepository.GetAllReviewsAsync(cancellationToken);
            return reviews.Select(r=>ReviewMapper.ToResponse(r)).ToList();
        }
    }
}
