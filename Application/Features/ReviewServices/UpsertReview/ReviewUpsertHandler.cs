using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Entity;
using Domain.Exceptions;
using Domain.Value_Object;
using MediatR;


namespace Application.Features.ReviewServices.UpsertReview
{
    public class ReviewUpsertHandler : IRequestHandler<ReviewUpsertCommand, ReviewResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IReviewRepository reviewRepository;
        private readonly IUserRepository userRepository;
        private readonly IMediaRepository mediaRepository;
        public ReviewUpsertHandler(IUnitOfWork unitOfWork, IReviewRepository reviewRepository, IUserRepository userRepository, IMediaRepository mediaRepository)
        {
            this.unitOfWork = unitOfWork;
            this.reviewRepository = reviewRepository;
            this.userRepository = userRepository;
            this.mediaRepository = mediaRepository;
        }
        public async Task<ReviewResponse> Handle(ReviewUpsertCommand request, CancellationToken cancellationToken)
        {
            Review? reviewDomain;
            if (request.id.HasValue)
            {
                reviewDomain = await reviewRepository.GetReviewByIdAsync(request.id.Value);
                if (reviewDomain is null)
                {
                    throw new NotFoundException($"Review for the given id: { request.id.Value } does not exist");
                }
                reviewDomain.Update(new Rating(request.Rating),request.Comment);
            }
            else
            {
                if(!request.userId.HasValue || !request.mediaId.HasValue)
                {
                    throw new ArgumentException("UserId and MediaId must be provided for creating a new review.");
                }
                var user = await userRepository.GetUserById(request.userId.Value);
                var media = await mediaRepository.GetMediaById(request.mediaId.Value);
                reviewDomain = Review.Create(new Rating(request.Rating), request.Comment, media.Id, user.Id);
                reviewDomain = await reviewRepository.AddAsync(reviewDomain);
            }
            await unitOfWork.CompleteAsync();
            return ReviewMapper.ToResponse(reviewDomain);
        }
    }
}
