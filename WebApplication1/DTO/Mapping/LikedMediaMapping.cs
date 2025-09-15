using WebApplication1.DTO.Response;
using WebApplication1.Models;

namespace WebApplication1.DTO.Mapping
{
    public class LikedMediaMapping
    {
        public static LikedMediaResponse ToResponse(LikedMedia likedMedia)
        {
            return new LikedMediaResponse(
                user: UserMapping.ToResponse(likedMedia.User),
                Media: MediaMapping.ToResponse(likedMedia.Media),
                likedMedia.LikedDate
            );
        }
    }
}
