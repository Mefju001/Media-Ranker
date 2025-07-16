using WebApplication1.DTO.Response;
using WebApplication1.Models;

namespace WebApplication1.DTO.Mapping
{
    public class LikedMovieMapping
    {
        public static LikedMovieResponse ToResponse(UserMovieLike userMovieLike)
        {
            return new LikedMovieResponse(
                user: UserMapping.ToResponse(userMovieLike.user),
                movie: MovieMapping.ToResponse(userMovieLike.movie),
                userMovieLike.LikedDate
            );
        }
    }
}
