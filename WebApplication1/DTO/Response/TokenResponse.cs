using WebApplication1.Models;

namespace WebApplication1.DTO.Response
{
    public record TokenResponse(string accessToken,string refreshToken);
}
