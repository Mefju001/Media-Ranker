using MediatR;
using WebApplication1.Application.Common.DTO.Request;
using WebApplication1.Application.Common.DTO.Response;

namespace WebApplication1.Application.Features.Movies.MovieUpsert
{
    public record UpsertMovieCommand(
        int? id,
        string Title,
        string Description,
        GenreRequest Genre,
        DirectorRequest Director,
        DateTime? ReleaseDate,
        string Language,
        TimeSpan Duration,
        bool IsCinemaRelease) : IRequest<MovieResponse>;
}
