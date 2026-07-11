using Application.Features.Common.Interfaces;
using MediatR;

namespace Application.Features.User.DeleteById
{
    public record DeleteByIdCommand(Guid id) : ICommand<Unit>;
}
