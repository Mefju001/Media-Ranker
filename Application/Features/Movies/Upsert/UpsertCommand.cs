using Application.Features.Common.DTO.Request;
using Application.Features.Common.Interfaces;
using Application.Features.Movies.Common;

namespace Application.Features.Movies.MovieUpsert
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
        bool IsCinemaRelease) : ICommand<MovieResponse>;
}
