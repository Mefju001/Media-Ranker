using Application.Common.DTO.Response;
using Domain.Entity;
using System.Linq.Expressions;

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
        public static Expression<Func<GenreDomain, GenreResponse>> ToDto = g => new GenreResponse
        (
            g.Id,
            g.name
        );

    }
}
