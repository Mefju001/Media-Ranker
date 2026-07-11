using Domain.Aggregate;

namespace Application.Features.Directors.Common
{
    public class DirectorMapper
    {
        public static DirectorResponse ToResponse(Director director)
        {
            return new DirectorResponse(
                director.Id,
                director.fullname.FirstName,
                director.fullname.LastName
               );
        }
    }
}
