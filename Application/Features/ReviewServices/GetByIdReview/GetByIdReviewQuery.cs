using Application.Common.DTO.Response;
using Application.Common.Interfaces;

namespace Application.Features.ReviewServices.GetByIdReview
{
    public record GetByIdReviewQuery(Guid reviewId) : IQuery<ReviewResponse?>;

}
