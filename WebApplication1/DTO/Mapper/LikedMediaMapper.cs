using WebApplication1.DTO.Response;
using WebApplication1.Models;

namespace WebApplication1.DTO.Mapper
{
    public class LikedMediaMapper
    {
        public static LikedMediaResponse ToResponse(LikedMedia likedMedia)
        {
            return new LikedMediaResponse(
                likedMedia.Id,
                UserMapper.ToResponse(likedMedia.User)
                ?? null,
                MediaMapper.ToResponse(likedMedia.Media)
                ?? null,
                likedMedia.LikedDate
            );
        }
    }
}
