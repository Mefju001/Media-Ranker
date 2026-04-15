using Application.Common.DTO.Request;
using Application.Common.DTO.Response;
using Application.Common.Interfaces;

namespace Application.Features.MovieServices.MovieUpsert
{
    public record UpsertMovieCommand(
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
