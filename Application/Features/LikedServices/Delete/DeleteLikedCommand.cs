using MediatR;

namespace Application.Features.LikedServices.Delete
{
    public record DeleteLikedCommand(int userId, int mediaId) : IRequest<bool>;
}
