using Application.Common.DTO.Response;
using Domain.Aggregate;
using Domain.Entity;

namespace Application.Mapper
{
    public static class MovieMapper
    {
        public static MovieResponse ToMovieResponse(Movie movieDomain, Genre genreDomain, Director director)
        {
            return new MovieResponse(
                movieDomain.Id,
                movieDomain.Title,
                movieDomain.Description,
                GenreMapper.ToResponse(genreDomain) ?? new GenreResponse(0, "Nie podano"),
                DirectorMapper.ToResponse(director) ?? new DirectorResponse(0, "Nie znany", "nie znany"),
                movieDomain.ReleaseDate.Value,
                movieDomain.Language,
                movieDomain.Reviews?.Select(r => ReviewMapper.ToResponse(r)).ToList() ?? new List<ReviewResponse>(),
                MediaStatsMapper.ToResponse(movieDomain.Stats) ?? new MediaStatsResponse(0, 0, null),
                movieDomain.Duration.Value,
                movieDomain.IsCinemaRelease);
        }

    }
}