using Application.Features.Common.Interfaces;
using MediatR;

namespace Application.Features.Liked.DeleteById
{
    public record DeleteByIdCommand(Guid userId, Guid mediaId) : ICommand<Unit>;
}
