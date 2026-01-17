using Application.Common.DTO.Response;
using Domain.Entity;

namespace Application.Mapper
{
    public class LikedMediaMapper
    {
        public static LikedMediaResponse ToResponse(LikedMediaDomain likedMedia)
        {
            return new LikedMediaResponse(
                likedMedia.id,
                //UserMapper.ToResponse(likedMedia.User)
                null,
                //MediaMapper.ToResponse(likedMedia.Media)
                null,
                likedMedia.likedDate
            );
        }
    }
}
