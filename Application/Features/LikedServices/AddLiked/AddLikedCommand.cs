using Application.Common.Interfaces;

namespace Application.Features.LikedServices.AddLiked
{
    public record AddLikedCommand(Guid UserId, Guid MediaId) : ICommand<bool>;
}
