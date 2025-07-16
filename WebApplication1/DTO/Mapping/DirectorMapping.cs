using WebApplication1.DTO.Response;
using WebApplication1.Models;

namespace WebApplication1.DTO.Mapping
{
    public class DirectorMapping
    {
        public static DirectorResponse ToResponse(Director director)
        {
            return new DirectorResponse(
                director.name,
                director.surname
               );
        }
    }
}
