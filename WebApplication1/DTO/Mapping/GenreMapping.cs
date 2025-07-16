using WebApplication1.DTO.Response;
using WebApplication1.Models;

namespace WebApplication1.DTO.Mapping
{
    public class GenreMapping
    {
        public static GenreResponse ToResponse(Genre genre)
        {
            return new GenreResponse(
                genre.name);
                }
    }
}
