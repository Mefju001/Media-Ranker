using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Exceptions;
using Domain.Repository;
using Domain.Value_Object;
using MediatR;


namespace Application.Features.ReviewServices.UpsertReview
{
    public class ReviewUpsertHandler : IRequestHandler<ReviewUpsertCommand, ReviewResponse>
    {
        private readonly IUserDetailsRepository userRepository;
        private readonly IMediaRepository<Media> mediaRepository;
        public ReviewUpsertHandler(IMediaRepository<Media> mediaRepository, IUserDetailsRepository userRepository)
        {
            this.userRepository = userRepository;
            this.mediaRepository = mediaRepository;
        }
        public async Task<ReviewResponse> Handle(ReviewUpsertCommand request, CancellationToken cancellationToken)
        {
            if (!request.userId.HasValue || !request.mediaId.HasValue)
            {
                throw new ArgumentException("UserId and MediaId must be provided.");
            }
            var media = await mediaRepository.GetByIdAsync(request.mediaId.Value, cancellationToken) ?? throw new NotFoundException($"Media with id {request.mediaId.Value} not found.");
            if (request.id.HasValue)
            {
                media.EditReview(request.id.Value, request.userId.Value, new Rating(request.Rating), request.Comment);
            }
            else
            {
                var username = await userRepository.GetUsernameById(request.userId.Value, cancellationToken);
                media.AddReview(request.userId.Value, new Rating(request.Rating), request.Comment, new Username(username));
            }
            var review = media.Reviews.First(r => r.UserId == request.userId.Value);
            return ReviewMapper.ToResponse(review);
        }
    }
}
