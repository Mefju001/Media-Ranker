using Application.Features.Common.Interfaces;

namespace Application.Features.Ignored.DeleteById
{
    public record DeleteByIdCommand(Guid userId, Guid mediaId):ICommand<bool>;
}
