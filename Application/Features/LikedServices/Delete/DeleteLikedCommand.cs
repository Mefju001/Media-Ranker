using MediatR;

namespace Application.Features.LikedServices.Delete
{
    public record DeleteLikedCommand(Guid userId, int mediaId) : IRequest<bool>;
}
