namespace Application.Features.Auth.Login
{
    public record LoginResponse(Guid userId, string username, string accessToken, string refreshToken);
}
