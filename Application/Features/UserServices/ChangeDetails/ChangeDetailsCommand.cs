using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.UserServices.ChangeDetails
{
    public record ChangeDetailsCommand(Guid userId, string name, string surname) : ICommand<Unit>;
}
