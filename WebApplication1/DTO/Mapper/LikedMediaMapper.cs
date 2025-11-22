using WebApplication1.DTO.Response;
using WebApplication1.Models;

namespace WebApplication1.DTO.Mapping
{
    public class LikedMediaMapper
    {
        public static LikedMediaResponse ToResponse(LikedMedia likedMedia)
        {
            return new LikedMediaResponse(
                user: UserMapper.ToResponse(likedMedia.User)
                ?? null,
                Media: MediaMapper.ToResponse(likedMedia.Media)
                ?? null,
                likedMedia.LikedDate
            );
        }
    }
}
