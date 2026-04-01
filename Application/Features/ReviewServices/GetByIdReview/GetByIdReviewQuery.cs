using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.ReviewServices.GetByIdReview
{
    public record GetByIdReviewQuery(int reviewId) : IQuery<ReviewResponse?>;

}
