using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Entity;
using Domain.Exceptions;
using MediatR;


namespace Application.Features.ReviewServices.UpsertReview
{
    public class ReviewUpsertHandler : IRequestHandler<ReviewUpsertCommand, ReviewResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        public ReviewUpsertHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<ReviewResponse> Handle(ReviewUpsertCommand request, CancellationToken cancellationToken)
        {
            ReviewDomain? reviewDomain;
            if (request.id.HasValue)
            {
                reviewDomain = await unitOfWork.ReviewRepository.GetReviewByIdAsync(request.id.Value);
                if (reviewDomain is null)
                {
                    throw new NotFoundException($"Review for the given id: { request.id.Value } does not exist");
                }
                reviewDomain.Update(request.Rating,request.Comment);
            }
            else
            {
                if(!request.userId.HasValue || !request.mediaId.HasValue)
                {
                    throw new ArgumentException("UserId and MediaId must be provided for creating a new review.");
                }
                var user = await unitOfWork.UserRepository.GetUserById(request.userId.Value);
                var media = await  unitOfWork.MediaRepository.GetMediaById(request.mediaId.Value);
                reviewDomain = ReviewDomain.Create(request.Rating,request.Comment, media.Id, user.Id);
                reviewDomain = await unitOfWork.ReviewRepository.AddAsync(reviewDomain);
            }
            await unitOfWork.CompleteAsync();
            return ReviewMapper.ToResponse(reviewDomain);
        }
    }
}
