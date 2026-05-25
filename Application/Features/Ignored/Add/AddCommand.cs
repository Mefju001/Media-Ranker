
using Application.Features.Common.Interfaces;

namespace Application.Features.Ignored.Add
{
    public record AddCommand(Guid mediaId,Guid userId):ICommand<bool>;
}
