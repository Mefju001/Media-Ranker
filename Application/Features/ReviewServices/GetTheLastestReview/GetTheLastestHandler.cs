using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.ReviewServices.GetTheLastestReview
{
    public class GetTheLastestHandler : IRequestHandler<GetTheLastestQuery, List<string>>
    {
        private readonly IMediaRepository<Media> mediaRepository;
        public GetTheLastestHandler(IMediaRepository<Media> mediaRepository)
        {
            this.mediaRepository = mediaRepository;
        }
        public async Task<List<string>> Handle(GetTheLastestQuery request, CancellationToken cancellationToken)
        {
            var reviewsId = await mediaRepository.GetTheLastestReviewAsync(cancellationToken);
            var TitleList = await mediaRepository.GetTitleByIdsAsync(reviewsId, cancellationToken);
            return TitleList;
        }
    }
}
