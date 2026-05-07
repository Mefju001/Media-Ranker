using Application.Features.Common.Interfaces;

namespace Application.Features.Liked.Delete
{
    public record DeleteCommand(Guid userId, Guid mediaId) : ICommand<bool>;
}
