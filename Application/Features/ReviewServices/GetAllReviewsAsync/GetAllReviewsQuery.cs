using Application.Common.DTO.Response;
using MediatR;

namespace Application.Features.ReviewServices.GetAllReviewsAsync
{
    public record GetAllReviewsQuery : IRequest<List<ReviewResponse>>;
}
