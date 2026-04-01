using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.ReviewServices.DeleteReviewAsync
{
    public record DeleteReviewCommand(int mediaId, int reviewId) : ICommand<bool>;
}
