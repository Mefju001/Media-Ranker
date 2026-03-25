using Application.Common.DTO.Response;
using MediatR;

namespace Application.Features.ReviewServices.GetByIdReview
{
    public record GetByIdReviewQuery(int reviewId) : IRequest<ReviewResponse?>;

}
