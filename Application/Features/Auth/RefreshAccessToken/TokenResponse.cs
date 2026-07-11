namespace Application.Features.Auth.RefreshAccessToken
{
    public record TokenResponse(string username, string accessToken, string refreshToken);
}
