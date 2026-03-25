namespace Application.Features.AuthServices.Signup
{
    public record SignUpResponse(string username, string? accessToken, string? refreshToken) { }
}
