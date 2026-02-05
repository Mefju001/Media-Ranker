using Application.Common.DTO.Response;
using Domain.Entity;

namespace Application.Mapper
{
    public class LikedMediaMapper
    {
        public static LikedMediaResponse ToResponse(LikedMediaDomain likedMedia,UserDomain userDomain,MovieDomain movieDomain,GenreDomain genreDomain,DirectorDomain director)
        {
            return new LikedMediaResponse(
                likedMedia.id,
                UserMapper.ToResponse(userDomain),
                MovieMapper.ToMovieResponse(movieDomain, genreDomain,director),
                likedMedia.likedDate
            );
        }
        public static LikedMediaResponse ToResponse(LikedMediaDomain likedMedia, UserDomain userDomain, GameDomain gameDomain, GenreDomain genreDomain)
        {
            return new LikedMediaResponse(
                likedMedia.id,
                UserMapper.ToResponse(userDomain),
                GameMapper.ToGameResponse(gameDomain, genreDomain),
                likedMedia.likedDate
            );
        }
        public static LikedMediaResponse ToResponse(LikedMediaDomain likedMedia, UserDomain userDomain, TvSeriesDomain tvSeriesDomain, GenreDomain genreDomain)
        {
            return new LikedMediaResponse(
                likedMedia.id,
                UserMapper.ToResponse(userDomain),
                TvSeriesMapper.ToTvSeriesResponse(tvSeriesDomain, genreDomain),
                likedMedia.likedDate
            );
        }
    }
}
