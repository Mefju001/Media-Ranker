using Application.Features.Common.Interfaces;
using MediatR;

namespace Application.Features.AdminPanel.ChangeDetails
{
    public record ChangeDetailsCommand(Guid userId, string name, string surname) : ICommand<Unit>;
}
