using Application.Features.Common.Models;
using Application.Features.User.Common;

namespace Application.Features.UserInteractions.Statuses.Common
{
    public record UserInteractionsResponse(Guid id, UserDetailsResponse user, MediaResponse MediaResponse, DateTime LikedDate);
}
