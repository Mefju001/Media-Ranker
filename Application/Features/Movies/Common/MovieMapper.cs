using Application.Features.Directors.Common;
using Application.Features.Genres.GetAll;
using Application.Features.Reviews.Common;
using Domain.Aggregate;

namespace Application.Features.Movies.Common
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
                movieDomain.DistributionType.ToString(),
                movieDomain.Status.ToString());
        }
        public static MovieResponse ToMovieResponse(Movie movieDomain, GenreResponse genreResponse, DirectorResponse directorResponse)
        {
            return new MovieResponse(
                movieDomain.Id,
                movieDomain.Title,
                movieDomain.Description,
                genreResponse,
                directorResponse,
                movieDomain.ReleaseDate.Value,
                movieDomain.Language,
                movieDomain.Reviews?.Select(r => ReviewMapper.ToResponse(r)).ToList() ?? new List<ReviewResponse>(),
                MediaStatsMapper.ToResponse(movieDomain.Stats) ?? new MediaStatsResponse(0, 0, null),
                movieDomain.Duration.Value,
                movieDomain.DistributionType.ToString(),
                movieDomain.Status.ToString());
        }

    }
}