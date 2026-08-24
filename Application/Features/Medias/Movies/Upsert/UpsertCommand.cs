using Application.Features.Common.Interfaces;
using Application.Features.Directors.Common;
using Application.Features.Genres.Common;
using Application.Features.Medias.Movies.Common;
using Domain.Enums;

namespace Application.Features.Medias.Movies.Upsert
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
        EDistributionType DistributionType,
        EMovieStatus MovieStatus
        ) : ICommand<MovieResponse>, ISendNotificationCommand;
}
