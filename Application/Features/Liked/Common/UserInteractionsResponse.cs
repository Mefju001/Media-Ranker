using Application.Features.User.Common;

namespace Application.Features.Liked.Common
{
    public record UserInteractionsResponse(Guid id, UserDetailsResponse user, MediaResponse MediaResponse, DateTime LikedDate)
    {
    }
}
