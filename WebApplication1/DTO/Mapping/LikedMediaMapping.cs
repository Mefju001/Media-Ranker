using WebApplication1.DTO.Response;
using WebApplication1.Models;

namespace WebApplication1.DTO.Mapping
{
    public class LikedMediaMapping
    {
        public static LikedMediaResponse ToResponse(LikedMedia likedMedia)
        {
            return new LikedMediaResponse(
                user: UserMapping.ToResponse(likedMedia.User)
                ?? null,
                Media: MediaMapping.ToResponse(likedMedia.Media)
                ?? null,
                likedMedia.LikedDate
            );
        }
    }
}
