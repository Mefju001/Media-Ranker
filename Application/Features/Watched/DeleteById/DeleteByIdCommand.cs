using Application.Features.Common.Interfaces;

namespace Application.Features.Watched.DeleteById
{
    public record DeleteByIdCommand(Guid mediaId, Guid userId) : ICommand<bool>;
}
