using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Domain.Entities;

namespace WebApplication1.Application.Mapper
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
