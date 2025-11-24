using WebApplication1.DTO.Response;
using WebApplication1.Models;

namespace WebApplication1.DTO.Mapper
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
