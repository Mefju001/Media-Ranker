using WebApplication1.DTO.Response;
using WebApplication1.Models;

namespace WebApplication1.DTO.Mapping
{
    public class MediaMapping
    {
        public static MediaResponse ToResponse(Media media)
        {
            if (media is null) return null;
            return new MediaResponse(
                media.title,
                media.description,
                GenreMapping.ToResponse(media.genre)?? new GenreResponse("Nieznany"),
                media.ReleaseDate,
                media.Language,
                media.Reviews?.Select(r => ReviewMapping.ToResponse(r)).ToList() ?? new List<ReviewResponse>());
        }
    }
}
