using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Domain.Entities;

namespace WebApplication1.Application.Mapper
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
