using Application.Features.Common.Interfaces;

namespace Application.Features.Liked.Add
{
    public record AddCommand(Guid UserId, Guid MediaId) : ICommand<bool>;
}
