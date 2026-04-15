using Application.Common.DTO.Response;
using Application.Common.Interfaces;

namespace Application.Features.ReviewServices.GetAllReviewsAsync
{
    public record GetAllReviewsQuery : IQuery<List<ReviewResponse>>;
}
