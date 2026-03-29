using Application.Common.DTO.Response;
using Domain.Aggregate;
using Domain.Entity;

namespace Application.Mapper
{
    public class LikedMediaMapper
    {
        public static LikedMediaResponse ToResponse(LikedMedia likedMedia, User userDomain, Movie movieDomain, Genre genreDomain, Director director)
        {
            return new LikedMediaResponse(
                likedMedia.Id,
                UserMapper.ToResponse(userDomain),
                MovieMapper.ToMovieResponse(movieDomain, genreDomain, director),
                likedMedia.LikedDate
            );
        }
        public static LikedMediaResponse ToResponse(LikedMedia likedMedia, User userDomain, Game gameDomain, Genre genreDomain)
        {
            return new LikedMediaResponse(
                likedMedia.Id,
                UserMapper.ToResponse(userDomain),
                GameMapper.ToGameResponse(gameDomain, genreDomain),
                likedMedia.LikedDate
            );
        }
        public static LikedMediaResponse ToResponse(LikedMedia likedMedia, User userDomain, TvSeries tvSeriesDomain, Genre genreDomain)
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
