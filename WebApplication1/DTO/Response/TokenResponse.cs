using WebApplication1.Models;

namespace WebApplication1.DTO.Response
{
    public record TokenResponse(string username, ICollection<RoleResponse>role,string token);
}
