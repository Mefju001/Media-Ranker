using Application.Common.DTO.Response;
using Application.Features.MovieServices.MovieUpsert;
using Domain.Entity;
using System.Linq.Expressions;

namespace Application.Mapper
{
    public static class MovieMapper
    {
        public static MovieResponse ToMovieResponse(MovieDomain movieDomain)
        {
            return new MovieResponse(
                movieDomain.Id,
                movieDomain.Title,
                movieDomain.Description,
                GenreMapper.ToResponse(movieDomain.GenreDomain) ?? new GenreResponse(0, "Nie podano"),
                DirectorMapper.ToResponse(movieDomain.DirectorDomain) ?? new DirectorResponse(0, "Nie znany", "nie znany"),
                movieDomain.ReleaseDate,
                movieDomain.Language,
                movieDomain.Reviews?.Select(r => ReviewMapper.ToResponse(r)).ToList() ?? new List<ReviewResponse>(),
                MediaStatsMapper.ToResponse(movieDomain.Stats) ?? new MediaStatsResponse(0, 0, 0, null),
                movieDomain.Duration,
                movieDomain.IsCinemaRelease);
        }
        public static Expression<Func<MovieDomain, MovieResponse>> ToDto = movie => new MovieResponse
        (
            movie.Id,
            movie.Title,
            movie.Description,
            new GenreResponse
            (
                movie.GenreDomain.Id,
                movie.GenreDomain.name
            ),
            new DirectorResponse(movie.DirectorDomain.Id, movie.DirectorDomain.name, movie.DirectorDomain.surname),
            movie.ReleaseDate,
            movie.Language,
            movie.Reviews.Select(r => ReviewMapper.ToResponse(r)).ToList(),
            MediaStatsMapper.ToResponse(movie.Stats),
            movie.Duration,
            movie.IsCinemaRelease
        );
    }
}