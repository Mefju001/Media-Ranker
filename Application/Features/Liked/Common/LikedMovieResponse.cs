using Application.Features.Common.DTO.Response;
using Application.Features.User.Common;

namespace Application.Features.Liked.Common
{
    public record LikedMediaResponse(Guid id, UserDetailsResponse user, MediaResponse Media, DateTime LikedDate)
    {
    }
}
