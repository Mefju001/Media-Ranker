using Application.Features.Common.Interfaces;
using Application.Features.Directors.Common;
using Application.Features.Genres.Common;
using Application.Features.Movies.Common;

namespace Application.Features.Movies.Upsert
{
    public record UpsertCommand(
        Guid? id,
        string Title,
        string Description,
        GenreRequest Genre,
        DirectorRequest Director,
        DateTime? ReleaseDate,
        string Language,
        TimeSpan Duration,
        string DistributionType,
        string MovieStatus
        ) : ICommand<MovieResponse>, ISendNotificationCommand;
}
