using Application.Common.Interfaces;

namespace Application.Features.ReviewServices.DeleteReviewAsync
{
    public record DeleteReviewCommand(Guid mediaId, Guid reviewId) : ICommand<bool>;
}
