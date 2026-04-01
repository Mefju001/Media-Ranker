using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using MediatR;

namespace Application.Features.ReviewServices.GetAllReviewsAsync
{
    public class GetAllReviewsHandler : IRequestHandler<GetAllReviewsQuery, List<ReviewResponse>>
    {
        private readonly IMediaRepository<Media> mediaRepository;
        public GetAllReviewsHandler(IMediaRepository<Media> mediaRepository)
        {
            this.mediaRepository = mediaRepository;
        }
        public async Task<List<ReviewResponse>> Handle(GetAllReviewsQuery request, CancellationToken cancellationToken)
        {
            var reviews = await mediaRepository.GetAllReviewsAsync(cancellationToken);
            return reviews.Select(r => ReviewMapper.ToResponse(r)).ToList();
        }
    }
}
