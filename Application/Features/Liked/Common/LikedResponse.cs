using Application.Features.User.Common;

namespace Application.Features.Liked.Common
{
    public record LikedResponse(Guid id, UserDetailsResponse user, MediaResponse MediaResponse, DateTime LikedDate)
    {
    }
}
//MovieResponse? MovieResponse, TvSeriesResponse? TvSeriesResponse, GameResponse? GameResponse