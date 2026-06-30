using Application.Features.Common.Interfaces;
using Application.Features.Directors.Common;
using Application.Features.Genres.Common;
using Application.Features.Movies.Common;
using Domain.Enums;

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
        bool IsCinemaRelease,
        EMovieStatus EMovieStatus) : ICommand<MovieResponse>, ISendNotificationCommand;
}
