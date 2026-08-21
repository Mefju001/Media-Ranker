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
            if (media is Movie movie)
                return ToResponse(likedMedia, user, movie, genre, director);

            if (media is Game game)
                return ToResponse(likedMedia, user, game, genre);

            if (media is domain.TvSeries tv)
                return ToResponse(likedMedia, user, tv, genre);

            throw new Exception("Brak danych o typie mediów");
        }
        private static UserInteractionsResponse ToResponse(Domain.Entity.UserInteractions likedMedia, UserDetails userDomain, Movie movieDomain, Genre genreDomain, Director director)
        {
            return new UserInteractionsResponse(
                likedMedia.Id,
                UserMapper.ToResponse(userDomain),
                MovieMapper.ToMovieResponse(movieDomain,genreDomain, director),
                likedMedia.InteractionDate
            );
        }
        private static UserInteractionsResponse ToResponse(Domain.Entity.UserInteractions likedMedia, UserDetails userDomain, Game gameDomain, Genre genreDomain)
        {
            return new UserInteractionsResponse(
                likedMedia.Id,
                UserMapper.ToResponse(userDomain),
                GameMapper.ToGameResponse(gameDomain, genreDomain),
                likedMedia.InteractionDate
            );
        }
        private static UserInteractionsResponse ToResponse(Domain.Entity.UserInteractions likedMedia, UserDetails userDomain, domain.TvSeries tvSeriesDomain, Genre genreDomain)
        {
            return new UserInteractionsResponse(
                likedMedia.Id,
                UserMapper.ToResponse(userDomain),
                TvSeriesMapper.ToTvSeriesResponse(tvSeriesDomain, genreDomain),
                likedMedia.InteractionDate
            );
        }
    }
}
