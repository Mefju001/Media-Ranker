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
        private readonly IUnitOfWork unitOfWork;
        private readonly IReviewRepository reviewRepository;
        private readonly IUserRepository userRepository;
        private readonly IMediaRepository mediaRepository;
        private readonly ILogger<ReviewUpsertHandler> logger;
        public ReviewUpsertHandler(IUnitOfWork unitOfWork, ILogger<ReviewUpsertHandler> logger, IReviewRepository reviewRepository, IUserRepository userRepository, IMediaRepository mediaRepository)
        {
            this.logger = logger;
            this.unitOfWork = unitOfWork;
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
                var userTask = userRepository.GetUserById(request.userId.Value, cancellationToken);
                var mediaTask = mediaRepository.GetMediaById(request.mediaId.Value, cancellationToken);
                await Task.WhenAll(userTask, mediaTask);
                var user = await userTask;
                var media = await mediaTask;
                if(user is null)
                {
                    throw new NotFoundException($"User with id {request.userId.Value} not found.");
                }
                if(media is null)
                {
                    throw new NotFoundException($"Media with id {request.mediaId.Value} not found.");
                }
                reviewDomain = Review.Create(new Rating(request.Rating), request.Comment, media.Id, user.Id);
                reviewDomain = await reviewRepository.AddAsync(reviewDomain, cancellationToken);
                logger.LogInformation("Created new review with id {ReviewId}", reviewDomain.Id);
            }
            await unitOfWork.CompleteAsync(cancellationToken);
            return ReviewMapper.ToResponse(reviewDomain);
        }
    }
}
