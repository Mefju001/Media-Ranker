namespace Application.Features.Auth.Signup
{
    public record SignUpResponse(string username, string? accessToken, string? refreshToken) { }
}
