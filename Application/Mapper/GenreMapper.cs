using Application.Common.DTO.Response;
using Domain.Entity;

namespace Application.Mapper
{
    public class GenreMapper
    {
        public static GenreResponse ToResponse(GenreDomain genre)
        {
            return new GenreResponse(
                genre.Id,
                genre.name);
        }

        
    }
}
