using Application.Features.Genres.GetGenres;
using Domain.Aggregate;

namespace Application.Features.Genres.GetAll
{
    public class GenreMapper
    {
        public static GenreResponse ToResponse(Genre genre)
        {
            return new GenreResponse(
                genre.Id,
                genre.Name);
        }
    }
}
