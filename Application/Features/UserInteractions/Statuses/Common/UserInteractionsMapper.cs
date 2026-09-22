using Application.Features.Common.Mapping;
using Application.Features.Medias.Games.Common;
using Application.Features.Medias.Movies.Common;
using Application.Features.Medias.TvSeries.Common;
using Application.Features.User.Common;
using Domain.Aggregate;
using domain = Domain.Aggregate;

namespace Application.Features.UserInteractions.Statuses.Common
{
    public class UserInteractionsMapper
    {
        public static UserInteractionsResponse ToResponse(
        Domain.Entity.UserInteractions likedMedia,
        UserDetails user,
        Media media,
        Genre genre,
        Director? director = null
        )
        {
            return new UserInteractionsResponse(
                likedMedia.Id,
                UserMapper.ToResponse(user),
                media.ToResponse(genre, director),
                likedMedia.InteractionDate);
        }
    }
}
