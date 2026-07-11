namespace Application.Features.Auth.Signup
{
    public record SignUpRequest(string username, string email, string password, string name, string surname);
}
