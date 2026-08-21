using Application.Features.Genres.GetAll;
using Application.Features.Medias.Movies.Common;

namespace Application.Features.Medias.PremieresAndAnnouncements.GetReleases
{
    public record ReleaseItemResponse(
        Guid Id,
        string Title,
        string Description,
        DateTime ReleaseDate,
        string MediaType,
        GenreResponse Genre,
        MediaStatsResponse MediaStatsResponse);
}
