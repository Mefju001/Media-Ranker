using MediatR;

namespace Application.Features.ReviewServices.DeleteReviewAsync
{
    public record DeleteReviewCommand(int reviewId) : IRequest<bool>;
}
