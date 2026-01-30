using Application.Common.DTO.Response;
using Domain.Entity;

namespace Application.Mapper
{
    public class MediaMapper
    {
        public static MediaResponse ToResponse(MediaDomain media)
        {
            if (media is null) throw new NullReferenceException("Media is null");
            return new MediaResponse(
                media.Id,
                media.Title,
                media.Description,
                new GenreResponse(9, ""),//GenreMapper.ToResponse(media.genre) ?? new GenreResponse(0, "Nieznany"),
                media.ReleaseDate,
                media.Language,
                media.Reviews?.Select(r => ReviewMapper.ToResponse(r)).ToList() ?? new List<ReviewResponse>());
        }
    }
}
