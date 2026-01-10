using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Domain.Entities;

namespace WebApplication1.Application.Mapper
{
    public class MediaMapper
    {
        public static MediaResponse ToResponse(Media media)
        {
            if (media is null) throw new NullReferenceException("Media is null");
            return new MediaResponse(
                media.Id,
                media.title,
                media.description,
                GenreMapper.ToResponse(media.genre) ?? new GenreResponse(0, "Nieznany"),
                media.ReleaseDate,
                media.Language,
                media.Reviews?.Select(r => ReviewMapper.ToResponse(r)).ToList() ?? new List<ReviewResponse>());
        }
    }
}
