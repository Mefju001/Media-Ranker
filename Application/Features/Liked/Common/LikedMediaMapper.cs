using Application.Features.Games.Common;
using Application.Features.Movie.Common;
using Application.Features.TvSeries.Common;
using Application.Features.User.Common;
using Domain.Aggregate;
using Domain.Entity;

namespace Application.Features.Liked.Common
{
    public class LikedMediaMapper
    {
        public static LikedMediaResponse ToResponse(
        UserInteractions likedMedia,
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

            if (media is TvSeries tv)
                return ToResponse(likedMedia, user, tv, genre);

            throw new Exception("Brak danych o typie mediów");
        }
        private static LikedMediaResponse ToResponse(UserInteractions likedMedia, UserDetails userDomain, Movie movieDomain, Genre genreDomain, Director director)
        {
            return new LikedMediaResponse(
                likedMedia.Id.MediaId,
                UserMapper.ToResponse(userDomain),
                MovieMapper.ToMovieResponse(movieDomain, genreDomain, director),
                likedMedia.InteractionDate
            );
        }
        private static LikedMediaResponse ToResponse(UserInteractions likedMedia, UserDetails userDomain, Game gameDomain, Genre genreDomain)
        {
            return new LikedMediaResponse(
                likedMedia.Id.MediaId,
                UserMapper.ToResponse(userDomain),
                GameMapper.ToGameResponse(gameDomain, genreDomain),
                likedMedia.InteractionDate
            );
        }
        private static LikedMediaResponse ToResponse(UserInteractions likedMedia, UserDetails userDomain, TvSeries tvSeriesDomain, Genre genreDomain)
        {
            return new LikedMediaResponse(
                likedMedia.Id.MediaId,
                UserMapper.ToResponse(userDomain),
                TvSeriesMapper.ToTvSeriesResponse(tvSeriesDomain, genreDomain),
                likedMedia.InteractionDate
            );
        }
    }
}
