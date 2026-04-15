using Application.Common.Interfaces;

namespace Application.Features.LikedServices.Delete
{
    public record DeleteLikedCommand(Guid userId, Guid mediaId) : ICommand<bool>;
}
