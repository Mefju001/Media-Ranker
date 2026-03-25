using Application.Common.DTO.Response;
using Domain.Entity;

namespace Application.Mapper
{
    public class DirectorMapper
    {
        public static DirectorResponse ToResponse(Director director)
        {
            return new DirectorResponse(
                director.Id,
                director.name,
                director.surname
               );
        }
    }
}
