using WebApplication1.DTO.Response;
using WebApplication1.Models;

namespace WebApplication1.DTO.Mapping
{
    public class MovieAVGMapping
    {
        public static MovieAVGResponse ToResponse(Movie movie,double Avarage)
        {
            return new MovieAVGResponse(
                movie.title,
                movie.description,
                GenreMapping.ToResponse(movie.genre),
                DirectorMapping.ToResponse(movie.director),
                movie.ReleaseDate,
                movie.Language,
                movie.Reviews?.Select(r => ReviewMapping.ToResponse(r)).ToList() ?? new List<ReviewResponse>(),
                movie.Duration,
                movie.IsCinemaRelease,
                Avarage
                );
        }
    }
}
