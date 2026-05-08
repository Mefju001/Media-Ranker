namespace Application.Features.User.Common
{
    public record UserDetailsResponse(Guid id, string username, string email, string name,
    string surname);
}
