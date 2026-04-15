using Application.Common.DTO.Response;
using Domain.Aggregate;

namespace Application.Mapper
{
    public class DirectorMapper
    {
        public static DirectorResponse ToResponse(Director director)
        {
            return new DirectorResponse(
                director.Id,
                director.fullname.Name,
                director.fullname.Surname
               );
        }
    }
}
