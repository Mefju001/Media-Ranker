using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.Models;
namespace WebApplication1.DTO.Mapping
{
    public static class MovieMapping
    {
        public static MovieResponse ToResponse(Movie movie)
        {
            return new MovieResponse(
                movie.title,
                movie.description,
                GenreMapping.ToResponse(movie.genre),
                DirectorMapping.ToResponse(movie.director),
                movie.ReleaseDate,
                movie.Language,
                movie.Reviews?.Select(r => ReviewMapping.ToResponse(r)).ToList() ?? new List<ReviewResponse>(),
                movie.Duration,
                movie.IsCinemaRelease);
        }
        public static void UpdateEntity(Movie movie,MovieRequest movieRequest,Director director,Genre genre)
        {
            movie.title = movieRequest.Title;
            movie.description = movieRequest.Description;
            movie.director = director;
            movie.genre = genre;
            movie.ReleaseDate = movieRequest.ReleaseDate.Value;
            movie.Language = movieRequest.Language;
            movie.IsCinemaRelease = movieRequest.IsCinemaRelease;
            movie.Duration = movieRequest.Duration;
        }
    }
}