using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Entity;
using Domain.Exceptions;
using Domain.Value_Object;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Application.Features.ReviewServices.UpsertReview
{
    public class ReviewUpsertHandler : IRequestHandler<ReviewUpsertCommand, ReviewResponse>
    {
        private readonly IMediaRepository<Media> reviewRepository;
        private readonly IUserRepository userRepository;
        private readonly IMediaRepository<Media> mediaRepository;
        private readonly ILogger<ReviewUpsertHandler> logger;
        public ReviewUpsertHandler(ILogger<ReviewUpsertHandler> logger, IMediaRepository<Media> reviewRepository, IUserRepository userRepository, IMediaRepository<Media> mediaRepository)
        {
            this.logger = logger;
            
            this.reviewRepository = reviewRepository;
            this.userRepository = userRepository;
            this.mediaRepository = mediaRepository;
        }
        public async Task<ReviewResponse> Handle(ReviewUpsertCommand request, CancellationToken cancellationToken)
        {
            Review? reviewDomain = null;
            if (request.id.HasValue)
            {
                reviewDomain = await reviewRepository.GetReviewByIdAsync(request.id.Value, cancellationToken);
            }
            if (reviewDomain is not null)
            {
                logger.LogInformation("Updating review with id {ReviewId}", reviewDomain.Id);
                reviewDomain.Update(new Rating(request.Rating), request.Comment);
            }
            else
            {
                if (!request.userId.HasValue || !request.mediaId.HasValue)
                {
                    throw new ArgumentException("UserId and MediaId must be provided for creating a new review.");
                }
                var user = await userRepository.GetUserById(request.userId.Value, cancellationToken);
                var media = await mediaRepository.GetByIdAsync(request.mediaId.Value, cancellationToken);
                if (user is null)
                {
                    throw new NotFoundException($"User with id {request.userId.Value} not found.");
                }
                if (media is null)
                {
                    throw new NotFoundException($"Media with id {request.mediaId.Value} not found.");
                }
                reviewDomain = Review.Create(new Rating(request.Rating), request.Comment, media.Id, user.Id, new Username(user.Username));
                media.AddReview(reviewDomain);
                logger.LogInformation("Created new review with id {ReviewId}", reviewDomain.Id);
            }
            
            return ReviewMapper.ToResponse(reviewDomain);
        }
    }
}
