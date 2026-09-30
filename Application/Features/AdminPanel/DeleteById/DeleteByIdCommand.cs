using Application.Features.Common.Interfaces;
using MediatR;

namespace Application.Features.AdminPanel.DeleteById
{
    public record DeleteByIdCommand(Guid userId) : ICommand<Unit>;
}
