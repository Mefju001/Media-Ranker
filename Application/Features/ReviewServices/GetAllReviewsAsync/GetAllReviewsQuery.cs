using Application.Common.DTO.Response;
using Domain.Entity;
using MediatR;

namespace Application.Features.ReviewServices.GetAllReviewsAsync
{
    public record GetAllReviewsQuery : IRequest<List<ReviewResponse>>;
}
