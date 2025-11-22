using WebApplication1.DTO.Response;
using WebApplication1.Models;

namespace WebApplication1.DTO.Mapping
{
    public class MediaMapper
    {
        public static MediaResponse ToResponse(Media media)
        {
            if (media is null) throw new NullReferenceException("Media is null");
            return new MediaResponse(
                media.title,
                media.description,
                GenreMapper.ToResponse(media.genre) ?? new GenreResponse("Nieznany"),
                media.ReleaseDate,
                media.Language,
                media.Reviews?.Select(r => ReviewMapper.ToResponse(r)).ToList() ?? new List<ReviewResponse>());
        }
    }
}
