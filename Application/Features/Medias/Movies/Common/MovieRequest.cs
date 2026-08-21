using Application.Features.Directors.Common;
using Application.Features.Genres.Common;

namespace Application.Features.Medias.Movies.Common
{
    public record MovieRequest(
        string Title,
        string Description,
        GenreRequest Genre,
        DirectorRequest Director,
        DateTime ReleaseDate,
        string Language,
        TimeSpan Duration,
        string DistributionType,
        string Status
        );

}
