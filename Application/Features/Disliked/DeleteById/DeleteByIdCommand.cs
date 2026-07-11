using Application.Features.Common.Interfaces;

namespace Application.Features.Disliked.DeleteById
{
    public record DeleteByIdCommand(Guid userId, Guid mediaId) :ICommand<bool>;
}
