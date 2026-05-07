using Application.Common.DTO.Response;
using Application.Features.Common.Mapper;
using Application.Features.Genres.GetAll;
using Application.Features.Reviews.Common;
using Domain.Aggregate;

namespace Application.Features.Movies.Common
{
    public static class MovieMapper
    {
        public static MovieResponse ToMovieResponse(Domain.Aggregate.Movie movieDomain, Domain.Aggregate.Genre genreDomain, Director director)
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