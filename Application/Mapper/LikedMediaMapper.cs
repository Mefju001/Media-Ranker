using Application.Common.DTO.Response;
using Domain.Aggregate;
using Domain.Entity;

namespace Application.Mapper
{
    public class LikedMediaMapper
    {
        public static LikedMediaResponse ToResponse(
        LikedMedia likedMedia,
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
        private static LikedMediaResponse ToResponse(LikedMedia likedMedia, UserDetails userDomain, Movie movieDomain, Genre genreDomain, Director director)
        {
            return new LikedMediaResponse(
                likedMedia.Id,
                UserMapper.ToResponse(userDomain),
                MovieMapper.ToMovieResponse(movieDomain, genreDomain, director),
                likedMedia.LikedDate
            );
        }
        private static LikedMediaResponse ToResponse(LikedMedia likedMedia, UserDetails userDomain, Game gameDomain, Genre genreDomain)
        {
            return new LikedMediaResponse(
                likedMedia.Id,
                UserMapper.ToResponse(userDomain),
                GameMapper.ToGameResponse(gameDomain, genreDomain),
                likedMedia.LikedDate
            );
        }
        private static LikedMediaResponse ToResponse(LikedMedia likedMedia, UserDetails userDomain, TvSeries tvSeriesDomain, Genre genreDomain)
        {
            return new LikedMediaResponse(
                likedMedia.Id,
                UserMapper.ToResponse(userDomain),
                TvSeriesMapper.ToTvSeriesResponse(tvSeriesDomain, genreDomain),
                likedMedia.LikedDate
            );
        }
    }
}
