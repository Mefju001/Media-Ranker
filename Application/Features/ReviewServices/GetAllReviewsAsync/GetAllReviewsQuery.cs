using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.ReviewServices.GetAllReviewsAsync
{
    public record GetAllReviewsQuery : IQuery<List<ReviewResponse>>;
}
