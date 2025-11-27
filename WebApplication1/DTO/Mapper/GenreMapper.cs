using WebApplication1.DTO.Response;
using WebApplication1.Models;

namespace WebApplication1.DTO.Mapper
{
    public class GenreMapper
    {
        public static GenreResponse ToResponse(Genre genre)
        {
            return new GenreResponse(
                genre.Id,
                genre.name);
        }
    }
}
