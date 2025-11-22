using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.Models;
namespace WebApplication1.DTO.Mapping
{
    public static class MovieMapper
    {
        public static MovieResponse ToMovieResponse(Movie movie)
        {
            return new MovieResponse(
                movie.Id,
                movie.title,
                movie.description,
                GenreMapper.ToResponse(movie.genre) ?? new GenreResponse("Nie podano"),
                DirectorMapper.ToResponse(movie.director) ?? new DirectorResponse("Nie znany", "nie znany"),
                movie.ReleaseDate,
                movie.Language,
                movie.Reviews?.Select(r => ReviewMapper.ToResponse(r)).ToList() ?? new List<ReviewResponse>(),
                MediaStatsMapper.ToResponse(movie.Stats) ?? new MediaStatsResponse(0, 0, null),
                movie.Duration,
                movie.IsCinemaRelease);
        }
        public static MovieAVGResponse ToMovieAVGResponse(Movie movie, double avg)
        {
            var movieResponse = ToMovieResponse(movie);
            return new MovieAVGResponse(movieResponse, avg);
        }
        public static void UpdateEntity(Movie movie, MovieRequest movieRequest, Director director, Genre genre)
        {
            movie.title = movieRequest.Title;
            movie.description = movieRequest.Description;
            movie.director = director;
            movie.genre = genre;
            movie.ReleaseDate = movieRequest.ReleaseDate!.Value;
            movie.Language = movieRequest.Language;
            movie.IsCinemaRelease = movieRequest.IsCinemaRelease;
            movie.Duration = movieRequest.Duration;
        }
    }
}