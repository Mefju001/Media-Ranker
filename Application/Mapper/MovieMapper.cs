using Application.Common.DTO.Response;
using Application.Features.MovieServices.MovieUpsert;
using Domain.Entity;

namespace Application.Mapper
{
    public static class MovieMapper
    {
        public static MovieResponse ToMovieResponse(MovieDomain movieDomain,GenreDomain genreDomain,DirectorDomain directorDomain)
        {
            return new MovieResponse(
                movieDomain.Id,
                movieDomain.Title,
                movieDomain.Description,
                GenreMapper.ToResponse(genreDomain) ?? new GenreResponse(0, "Nie podano"),
                DirectorMapper.ToResponse(directorDomain) ?? new DirectorResponse(0, "Nie znany", "nie znany"),
                movieDomain.ReleaseDate,
                movieDomain.Language,
                movieDomain.Reviews?.Select(r => ReviewMapper.ToResponse(r)).ToList() ?? new List<ReviewResponse>(),
                MediaStatsMapper.ToResponse(movieDomain.Stats) ?? new MediaStatsResponse(0, 0, 0, null),
                movieDomain.Duration,
                movieDomain.IsCinemaRelease);
        }
    }
}