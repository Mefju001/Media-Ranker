using Application.Common.DTO.Response;
using Domain.Entity;

namespace Application.Mapper
{
    public class GenreMapper
    {
        public static GenreResponse ToResponse(Genre genre)
        {
            return new GenreResponse(
                genre.Id,
                genre.Name.Value);
        }
    }
}
