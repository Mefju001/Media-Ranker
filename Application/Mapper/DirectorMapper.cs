using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Domain.Entities;

namespace WebApplication1.Application.Mapper
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
