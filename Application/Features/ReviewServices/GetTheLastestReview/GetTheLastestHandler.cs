using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.ReviewServices.GetTheLastestReview
{
    public class GetTheLastestHandler : IRequestHandler<GetTheLastestQuery, List<string>>
    {
        private readonly IReviewRepository reviewRepository;
        public GetTheLastestHandler(IReviewRepository reviewRepository)
        {
            this.reviewRepository = reviewRepository;
        }
        public async Task<List<string>> Handle(GetTheLastestQuery request, CancellationToken cancellationToken)
        {
            var review = await reviewRepository.GetTheLastestReviewAsync(cancellationToken);
            return review;
        }
    }
}
