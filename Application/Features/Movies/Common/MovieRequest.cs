using Application.Features.Directors.Common;
using Application.Features.Genres.Common;
using Domain.Enums;

namespace Application.Features.Movies.Common
{
    public record MovieRequest(
        string Title,
        string Description,
        GenreRequest Genre,
        DirectorRequest Director,
        DateTime ReleaseDate,
        string Language,
        TimeSpan Duration,
        EDistributionType DistributionType,
        EMovieStatus Status
        );

}
