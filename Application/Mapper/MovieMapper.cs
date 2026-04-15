using Application.Common.DTO.Response;
using Domain.Aggregate;

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
                GenreMapper.ToResponse(genreDomain),
                DirectorMapper.ToResponse(director),
                movieDomain.ReleaseDate.Value,
                movieDomain.Language,
                movieDomain.Reviews?.Select(r => ReviewMapper.ToResponse(r)).ToList() ?? new List<ReviewResponse>(),
                MediaStatsMapper.ToResponse(movieDomain.Stats) ?? new MediaStatsResponse(0, 0, null),
                movieDomain.Duration.Value,
                movieDomain.IsCinemaRelease);
        }

    }
}