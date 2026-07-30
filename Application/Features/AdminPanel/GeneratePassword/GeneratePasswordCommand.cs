using Application.Features.Common.Interfaces;
using MediatR;

namespace Application.Features.AdminPanel.GeneratePassword
{
    public record GeneratePasswordCommand(Guid userId) : ICommand<Unit>;
}
