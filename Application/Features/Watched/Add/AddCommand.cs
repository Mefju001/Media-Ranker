using Application.Features.Common.Interfaces;

namespace Application.Features.Watched.Add
{
    public record AddCommand(Guid userId,Guid mediaId) : ICommand<bool>;
}
