using Application.Features.Common.Interfaces;

namespace Application.Features.Disliked.Add
{
    public record AddCommand(Guid UserId, Guid MediaId) :ICommand<bool>;
}
