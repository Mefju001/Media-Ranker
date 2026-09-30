using Application.Features.Common.Interfaces;

namespace Application.Features.UserInteractions.Reviews.DeleteById
{
    public record DeleteByIdCommand(Guid mediaId, Guid reviewId) : ICommand<bool>;
}
